using System;
using System.Collections.Generic;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Common.Logging;
using Common.Logging.Simple;
using NCmdLiner.SolutionCreator.Library.BootStrap;
using NCmdLiner.SolutionCreator.Library.Common;
using NCmdLiner.SolutionCreator.Library.Model;
using NCmdLiner.SolutionCreator.Library.Services;
using NCmdLiner.SolutionCreator.Library.Tests.Infrastructure;
using NCmdLiner.SolutionCreator.Library.ViewModels;
using NCmdLiner.SolutionCreator.Library.Views;
using NUnit.Framework;
using Rhino.Mocks;

namespace NCmdLiner.SolutionCreator.Library.Tests.ManualTests
{
    [TestFixture(Category = "ManualTests")]
    public class CreateSolutionApplicationTests
    {
        private ConsoleOutLogger _logger;

        [SetUp]
        public void SetUp()
        {
            _logger = new ConsoleOutLogger(this.GetType().Name, LogLevel.All, true, false, false, "yyyy-MM-dd hh:mm:ss");            
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test, RequiresSTA]
        public void CreateSolutionApplicationTest()
        {
            using(var testBooStrapper = new TestBootStrapper(GetType()))
            {
                var solutionTemplateProviderStub = testBooStrapper.Container.Resolve<ISolutionTemplateProvider>();
                solutionTemplateProviderStub.Stub(i => i.GetSolutionTemplates()).Return(new List<SolutionTemplate>()
                {
                    new SolutionTemplate() {Name="Console Application", Path = @"C:\Dev\github\NCmdLiner.SolutionCreator\src\NCmdLiner.SolutionTemplates\\Console Application"} ,
                    new SolutionTemplate() {Name="Client Service Application", Path = @"C:\Dev\github\NCmdLiner.SolutionCreator\src\NCmdLiner.SolutionTemplates\Client Service Application"}                
                }).Repeat.AtLeastOnce();
                var target = testBooStrapper.Container.Resolve<ICreateSolutionApplication>();
                target.InitializeAndRun(@"c:\temp");                
            }
        }

        internal class TestBootStrapper: IDisposable
        {
            readonly ILog _logger;
            private IWindsorContainer _container;

            public TestBootStrapper(Type type)
            {
                _logger = new ConsoleOutLogger(type.Name,LogLevel.Info, true, false,false,"yyyy-MM-dd HH:mm:ss");
            }

            public IWindsorContainer Container
            {
                get
                {
                    if(_container == null)
                    {
                        _container = new WindsorContainer();
                        _container.Register(Component.For<IWindsorContainer>().Instance(_container));

                        _container.AddFacility<TypedFactoryFacility>();
                        _container.Register(Component.For<ITypedFactoryComponentSelector>().ImplementedBy<CustomTypeFactoryComponentSelector>());
            
                        //Configure logging
                        _container.Register(Component.For<ILog>().Instance(_logger));

                        //Manual override registrations for interfaces that the interface under test is dependent on
                        _container.Register(Component.For<ISolutionTemplateProvider>().Instance(MockRepository.GenerateStub<ISolutionTemplateProvider>()));

                        _container.Register(Component.For<ISelectSolutionTemplateWindowFactory>().AsFactory());
                        _container.Register(
                            Component.For<SelectSolutionTemplateWindow>()
                                .ImplementedBy<SelectSolutionTemplateWindow>()
                                .Named(typeof(SelectSolutionTemplateWindow).Name)
                                .LifeStyle.Transient);
                        _container.Register(Component.For<SelectSolutionTemplateView>().Activator<StrictComponentActivator>());
                        _container.Register(Component.For<SelectSolutionTemplateViewModel>().Activator<StrictComponentActivator>());

                        _container.Register(Component.For<ISolutionInfoWindowFactory>().AsFactory());
                        _container.Register(
                            Component.For<SolutionInfoWindow>()
                                .ImplementedBy<SolutionInfoWindow>()
                                .Named(typeof(SolutionInfoWindow).Name)
                                .LifeStyle.Transient);
                        _container.Register(Component.For<SolutionInfoView>().Activator<StrictComponentActivator>());
                        _container.Register(Component.For<SolutionInfoViewModel>().Activator<StrictComponentActivator>());

                        _container.Register(Classes.FromAssemblyInThisApplication().IncludeNonPublicTypes().BasedOn<ITypeMapperConfiguration>().WithService.FromInterface());

                        _container.Kernel.Resolver.AddSubResolver(new CollectionResolver(_container.Kernel));


                        ///////////////////////////////////////////////////////////////////
                        //Automatic registrations
                        ///////////////////////////////////////////////////////////////////
                        //
                        //   Register all command providers and attach logging interceptor
                        //
                        const string libraryRootNameSpace = "NCmdLiner.SolutionCreator.Library";
                        
                        //
                        //   Register all singletons found in the library
                        //
                        _container.Register(Classes.FromAssemblyContaining<CommandDefinition>()
                            .InNamespace(libraryRootNameSpace, true)
                            .If(type => Attribute.IsDefined(type, typeof(SingletonAttribute)))
                            .WithService.DefaultInterfaces().LifestyleSingleton());
                        
                        //
                        //   Register all transients found in the library
                        //
                        _container.Register(Classes.FromAssemblyContaining<CommandDefinition>()
                            .InNamespace(libraryRootNameSpace, true)
                            .WithService.DefaultInterfaces().LifestyleTransient());

                    }
                    return _container;
                }

            }

            ~TestBootStrapper()
            {
                Dispose(false);
            }

            public void Dispose()
            {
                Dispose(true);
            }

            protected virtual void Dispose(bool disposing)
            {
                if(disposing)
                {
                    if(_container != null)
                    {
                        _container.Dispose();
                        _container = null;
                    }
                }
            }
        }
    }
}