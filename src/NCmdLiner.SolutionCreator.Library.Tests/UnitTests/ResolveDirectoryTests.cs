using System;
using System.IO;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Common.Logging;
using Common.Logging.Simple;
using NCmdLiner.SolutionCreator.Library.Common;
using NCmdLiner.SolutionCreator.Library.Common.IO;
using NCmdLiner.SolutionCreator.Library.Services;
using NUnit.Framework;
using Rhino.Mocks;

namespace NCmdLiner.SolutionCreator.Library.Tests.UnitTests
{
    [TestFixture(Category = "UnitTests")]
    public class ResolveDirectoryTests
    {
        private ILog _logger;
        private string _sourceTestFolder;
        private string _targetTestFolder;
        private string _sourceSubTestFolder;
        private string _sourceSubSubTestFolder;
        private string _sourceSubSubTestFolderFile;
        private string _sourceSubTestFolderFile;
        private string _targetSubTestFolderFile;
        private string _targetSubSubTestFolderFile;
        private string _sourceSubSubSubTestFolder;

        [SetUp]
        public void SetUp()
        {
            _logger = new ConsoleOutLogger(this.GetType().Name, LogLevel.All, true, false, false, "yyyy-MM-dd hh:mm:ss");
            _sourceTestFolder = Path.Combine(Path.GetTempPath(), "ResolveFolderTestsSource");
            _targetTestFolder = Path.Combine(Path.GetTempPath(), "ResolveFolderTestsTarget");
            TearDown();
            _sourceSubTestFolder = Path.Combine(_sourceTestFolder, "Some Folder for _S_ConsoleProjectName_S_ project");
            _sourceSubSubTestFolder = Path.Combine(_sourceSubTestFolder, "Sub Folder _S_LibraryProjectName_S_ Project");
            _sourceSubSubSubTestFolder = Path.Combine(_sourceSubSubTestFolder, "Sub Folder _S_SomeUnknown_S_ Project");
            TestData.CreateFolder(_sourceSubSubSubTestFolder);
            _sourceSubTestFolderFile = Path.Combine(_sourceSubTestFolder, "Test file _S_LibraryProjectName_S_ Project.txt");
            _sourceSubSubTestFolderFile = Path.Combine(_sourceSubSubTestFolder, "Test file _S_ConsoleProjectName_S_ Project.txt");
            TestData.CreateTestTextFile(_sourceSubTestFolderFile, Encoding.UTF8, "Testing _S_LibraryProjectName_S_ Project");
            TestData.CreateTestTextFile(_sourceSubSubTestFolderFile, Encoding.UTF8, "Testing _S_ConsoleProjectName_S_ Project");
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_sourceTestFolder))
            {
                Directory.Delete(_sourceTestFolder, true);
            }
            if (Directory.Exists(_targetTestFolder))
            {
                Directory.Delete(_targetTestFolder, true);
            }
        }

        [Test]
        public void ResolveFolder()
        {
            using (var testBooStrapper = new TestBootStrapper(GetType()))
            {
                var stubResolveContext = testBooStrapper.Container.Resolve<IResolveContext>();
                stubResolveContext.Stub(context => context.GetVariable("_S_ConsoleProjectName_S_")).Return("My.Football.Manager");
                stubResolveContext.Stub(context => context.GetVariable("_S_LibraryProjectName_S_")).Return("My.Football.Manager.Library");
                stubResolveContext.Stub(context => context.GetVariable("_S_ProjectDescription_S_")).Return("application for management of football teams.");

                var textResolver = testBooStrapper.Container.Resolve<ITextResolver>();
                _targetSubTestFolderFile = textResolver.Resolve(_sourceSubTestFolderFile).Replace(_sourceTestFolder,_targetTestFolder);
                _targetSubSubTestFolderFile = textResolver.Resolve(_sourceSubSubTestFolderFile).Replace(_sourceTestFolder, _targetTestFolder);
                
                Assert.IsTrue(File.Exists(_sourceSubTestFolderFile), "File does not exist:" + _sourceSubTestFolderFile);
                Assert.IsTrue(File.Exists(_sourceSubSubTestFolderFile), "File does not exist:" + _sourceSubSubTestFolderFile);
                var target = testBooStrapper.Container.Resolve<IFolderResolver>();
                target.Resolve(_sourceTestFolder, _targetTestFolder);
                Assert.IsTrue(File.Exists(_targetSubTestFolderFile), "File does not exist:" + _targetSubTestFolderFile);
                Assert.IsTrue(File.Exists(_targetSubSubTestFolderFile), "File does not exist:" + _targetSubSubTestFolderFile);
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

                        //Configure logging
                        _container.Register(Component.For<ILog>().Instance(_logger));

                        //Manual override registrations for interfaces that the interface under test is dependent on
                        _container.Register(Component.For<IResolveContext>().Instance(MockRepository.GenerateStub<IResolveContext>()));

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