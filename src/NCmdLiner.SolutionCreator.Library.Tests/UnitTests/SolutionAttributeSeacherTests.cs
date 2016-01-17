using System;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Common.Logging;
using Common.Logging.Simple;
using NCmdLiner.SolutionCreator.Library.Common;
using NCmdLiner.SolutionCreator.Library.Services;
using NUnit.Framework;

namespace NCmdLiner.SolutionCreator.Library.Tests.UnitTests
{
    [TestFixture(Category = "UnitTests")]
    public class SolutionAttributeSeacherTests
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

        [Test]
        public void FindSolutionAttributesFromStringTest1()
        {
            
            using(var testBooStrapper = new TestBootStrapper(GetType()))
            {
                var target = testBooStrapper.Container.Resolve<ISolutionAttributeSearcher>();
                const string text = "<ProductName>_S_ShortProductName_S_</ProductName>";
                var actual = target.FindSolutionAttributesFromString(text).ToList();
                CollectionAssert.IsNotEmpty(actual);
                Assert.AreEqual(1,actual.Count);
                Assert.AreEqual("_S_ShortProductName_S_", actual[0].Name);
            }
        }

        [Test]
        public void FindSolutionAttributesFromStringTest2()
        {            
            using(var testBooStrapper = new TestBootStrapper(GetType()))
            {
                var target = testBooStrapper.Container.Resolve<ISolutionAttributeSearcher>();
                const string text = "Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"_S_ServiceProjectName_S_\", \"src\\_S_SetupProjectName_S_\\_S_ConsoleProjectName_S_.csproj\", \"{E76AD126-A1F7-4F16-8B1A-2CF11E488C15}\"";
                var actual = target.FindSolutionAttributesFromString(text).ToList();
                CollectionAssert.IsNotEmpty(actual);
                Assert.AreEqual(3,actual.Count);
                Assert.AreEqual("_S_ServiceProjectName_S_", actual[0].Name);
                Assert.AreEqual("_S_SetupProjectName_S_", actual[1].Name);
                Assert.AreEqual("_S_ConsoleProjectName_S_", actual[2].Name);
            }
        }

        [Test]
        public void FindSolutionAttributesFromStringTest3()
        {
            
            using(var testBooStrapper = new TestBootStrapper(GetType()))
            {
                var target = testBooStrapper.Container.Resolve<ISolutionAttributeSearcher>();
                const string text = "Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"_S_ServiceProjectName_S_\", \"src\\_S_SetupProjectName_S_\\_S_ConsoleProjectName_S_.csproj\", \"{E76AD126-A1F7-4F16-8B1A-2CF11E488C15}\"";
                var actual = target.FindSolutionAttributesFromString(text).ToList();
                CollectionAssert.IsNotEmpty(actual);
                Assert.AreEqual(3,actual.Count);
                Assert.AreEqual("_S_ServiceProjectName_S_", actual[0].Name);
                Assert.AreEqual("Service Project Name", actual[0].DisplayName);

                Assert.AreEqual("_S_SetupProjectName_S_", actual[1].Name);
                Assert.AreEqual("Setup Project Name", actual[1].DisplayName);

                Assert.AreEqual("_S_ConsoleProjectName_S_", actual[2].Name);
                Assert.AreEqual("Console Project Name", actual[2].DisplayName);
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
            
                        //Configure logging
                        _container.Register(Component.For<ILog>().Instance(_logger));
            
                        //Manual override registrations for interfaces that the interface under test is dependent on
                        //_container.Register(Component.For<ISomeInterface>().Instance(MockRepository.GenerateStub<ISomeInterface>()));

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