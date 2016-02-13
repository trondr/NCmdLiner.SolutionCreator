using System;
using System.IO;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Common.Logging;
using Common.Logging.Simple;
using NCmdLiner.SolutionCreator.Library.Common;
using NCmdLiner.SolutionCreator.Library.Common.IO;
using NUnit.Framework;

namespace NCmdLiner.SolutionCreator.Library.Tests.UnitTests
{
    [TestFixture(Category = "UnitTests")]
    public class IniFileOperationTests
    {
        private ConsoleOutLogger _logger;
        private string _testIniFile;

        [SetUp]
        public void SetUp()
        {
            _logger = new ConsoleOutLogger(this.GetType().Name, LogLevel.All, true, false, false, "yyyy-MM-dd hh:mm:ss");
            _testIniFile = CreateTestIniFile();
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_testIniFile))
            {
                File.Delete(_testIniFile);
            }
        }

        [Test]
        public void GetSectionsTest()
        {

            using (var testBooStrapper = new TestBootStrapper(GetType()))
            {
                var target = testBooStrapper.Container.Resolve<IIniFileOperation>();
                
                var actual = target.GetSections(_testIniFile).ToList();

                Assert.AreEqual(3, actual.Count, "Number of sections in ini file was not expected");
                Assert.AreEqual("TestSection1", actual[0], "Section name not expected");
                Assert.AreEqual("TestSection2", actual[1], "Section name not expected");
                Assert.AreEqual("TestSection3", actual[2], "Section name not expected");
            }
        }

        [Test]
        public void GetSectionKeysTest()
        {

            using (var testBooStrapper = new TestBootStrapper(GetType()))
            {
                var target = testBooStrapper.Container.Resolve<IIniFileOperation>();
                
                var actual = target.GetKeys(_testIniFile, "TestSection1").ToList();

                Assert.AreEqual(3, actual.Count, "Number of sections in ini file was not expected");
                Assert.AreEqual("TestKey11", actual[0], "Section name not expected");
                Assert.AreEqual("TestKey12", actual[1], "Section name not expected");
                Assert.AreEqual("TestKey13", actual[2], "Section name not expected");
            }
        }


        private string CreateTestIniFile()
        {
            var iniFile = Path.GetFullPath(Environment.ExpandEnvironmentVariables("%temp%\\testinifile.ini"));
            using (var sw = new StreamWriter(iniFile, false))
            {
                sw.WriteLine("[TestSection1]");
                sw.WriteLine("TestKey11=Testvalue11");
                sw.WriteLine("");
                sw.WriteLine("TestKey12=Testvalue12");
                sw.WriteLine("TestKey13=Testvalue13");

                sw.WriteLine("[TestSection2]");
                sw.WriteLine("TestKey21=Testvalue21");
                sw.WriteLine("");
                sw.WriteLine("TestKey22=Testvalue22");
                sw.WriteLine("TestKey23=Testvalue23");

                sw.WriteLine("[TestSection3]");
                sw.WriteLine("TestKey31=Testvalue31");
                sw.WriteLine("");
                sw.WriteLine("TestKey32=Testvalue32");
                sw.WriteLine("TestKey33=Testvalue33");
            }
            return iniFile;
        }

        private string CreateTestIniFileWin1000Sections()
        {
            var iniFile = Path.GetFullPath(Environment.ExpandEnvironmentVariables("%temp%\\testinifile.ini"));
            using (var sw = new StreamWriter(iniFile, false))
            {
                sw.WriteLine("[TestSection1]");
                sw.WriteLine("TestKey11=Testvalue11");
                sw.WriteLine("");
                sw.WriteLine("TestKey12=Testvalue12");
                sw.WriteLine("TestKey13=Testvalue13");
                
            }
            return iniFile;
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