using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Common.Logging;
using Common.Logging.Simple;
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
    [TestFixture(Category = "ManualTests"), RequiresSTAAttribute]
    public class SelectSolutionTemplateWindowTests
    {
        private ConsoleOutLogger _logger;

        [SetUp]
        public void SetUp()
        {
            _logger = new ConsoleOutLogger(this.GetType().Name, LogLevel.All, true, false, false, "yyyy-MM-dd hh:mm:ss");
            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {
                throw new ThreadStateException("The current threads apartment state is not STA");
            }
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void SelectSolutionTemplateWindowTestCreate()
        {
            using (var testBooStrapper = new TestBootStrapper(GetType()))
            {
                var target = testBooStrapper.Container.Resolve<ISelectSolutionTemplateWindowFactory>();
                var actual = target.GetSelectSolutionTemplateWindow();
                Assert.IsNotNull(actual, "Window is null");
                Assert.AreEqual(actual.GetType(), typeof(SelectSolutionTemplateWindow), "Not correct type.");
                Assert.IsNotNull(actual.View, "View is null");
                Assert.AreEqual(actual.View.GetType(), typeof(SelectSolutionTemplateView), "Not correct type.");
                Assert.IsNotNull(actual.View.ViewModel, "ViewModel is null");
                Assert.AreEqual(actual.View.ViewModel.GetType(), typeof(SelectSolutionTemplateViewModel), "Not correct type.");
                target.Release(actual);
            }
        }

        [Test]
        public void SelectSolutionTemplateWindowTestOpenWindow()
        {
            using (var testBooStrapper = new TestBootStrapper(GetType()))
            {
                var windowFactory = testBooStrapper.Container.Resolve<ISelectSolutionTemplateWindowFactory>();
                var stubTemplateProvider = testBooStrapper.Container.Resolve<ISolutionTemplateProvider>();
                ////Prepare interface(s) that interface under test is dependent on by 
                ////letting the dependent (stub) interface(s) return dummy data to the interface under test
                stubTemplateProvider.SolutionTemplates = new List<SolutionTemplate>()
                {
                    new SolutionTemplate() {Name = "Console Application", Path = @"c:\temp\Templates\Console Application"},
                    new SolutionTemplate() {Name = "Client Service Application", Path = @"c:\temp\Templates\Client Service Application"},
                };

                var actual = windowFactory.GetSelectSolutionTemplateWindow();
                foreach (var solutionTemplate in stubTemplateProvider.SolutionTemplates)
                {
                    actual.View.ViewModel.SolutionTemplates.Add(new SolutionTemplateViewModel() { Name = solutionTemplate.Name, Path = solutionTemplate.Path });
                }
                Application application = new Application();
                application.Run(actual);
                _logger.Info("Dialog result: " + actual.DialogResult);

            }
        }

        internal class TestBootStrapper : IDisposable
        {
            readonly ILog _logger;
            private IWindsorContainer _container;

            public TestBootStrapper(Type type)
            {
                _logger = new ConsoleOutLogger(type.Name, LogLevel.Info, true, false, false, "yyyy-MM-dd HH:mm:ss");
            }

            public IWindsorContainer Container
            {
                get
                {
                    if (_container == null)
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

                        //Factory registrations example:

                        //container.Register(Component.For<ITeamProviderFactory>().AsFactory());
                        //container.Register(
                        //    Component.For<ITeamProvider>()
                        //        .ImplementedBy<CsvTeamProvider>()
                        //        .Named("CsvTeamProvider")
                        //        .LifeStyle.Transient);
                        //container.Register(
                        //    Component.For<ITeamProvider>()
                        //        .ImplementedBy<SqlTeamProvider>()
                        //        .Named("SqlTeamProvider")
                        //        .LifeStyle.Transient);

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
                if (disposing)
                {
                    if (_container != null)
                    {
                        _container.Dispose();
                        _container = null;
                    }
                }
            }
        }
    }
}