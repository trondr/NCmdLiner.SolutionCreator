﻿using System;
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
    public class NewResolveFileTests
    {
        private ConsoleOutLogger _logger;
        private IResolveContext _stubResolveContext;
        private string _sourceTestFile;
        private FileEncoding _fileEncoding;
        private TextResolver _textResolver;
        private string _targetTestFile;
        private IFileComparer _fileComparer;
        private FileCopy _fileCopy;


        [SetUp]
        public void SetUp()
        {
            _logger = new ConsoleOutLogger(this.GetType().Name, LogLevel.All, true, false, false, "yyyy-MM-dd hh:mm:ss");
            _logger = new ConsoleOutLogger(this.GetType().Name, LogLevel.All, true, false, false, "yyyy-MM-dd hh:mm:ss");
            _stubResolveContext = MockRepository.GenerateStub<IResolveContext>();
            _stubResolveContext.Stub(context => context.GetVariable("ConsoleProjectName")).Return("My.Football.Manager");
            _stubResolveContext.Stub(context => context.GetVariable("LibraryProjectName")).Return("My.Football.Manager.Library");
            _stubResolveContext.Stub(context => context.GetVariable("ProjectDescription")).Return("application for management of football teams.");
            _fileEncoding = new FileEncoding();
            _textResolver = new TextResolver(_stubResolveContext, _logger);
            _sourceTestFile = Path.GetTempFileName();
            _targetTestFile = Path.GetTempFileName();
            if (File.Exists(_sourceTestFile))
            {
                File.Delete(_sourceTestFile);
            }
            if (File.Exists(_targetTestFile))
            {
                File.Delete(_targetTestFile);
            }
            _fileComparer = new FileComparer(new FileTimeComparer(_logger), _logger);
            _fileCopy = new FileCopy();
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_sourceTestFile))
            {
                File.Delete(_sourceTestFile);
            }
            if (File.Exists(_targetTestFile))
            {
                File.Delete(_targetTestFile);
            }
        }

        [Test]
        public void ResolveFileAllVariablesKnownTest()
        {




            using (var testBooStrapper = new TestBootStrapper(GetType()))
            {
                var stubResolveContext = testBooStrapper.Container.Resolve<IResolveContext>();
                stubResolveContext.Stub(context => context.GetVariable("_S_ConsoleProjectName_S_")).Return("My.Football.Manager");
                stubResolveContext.Stub(context => context.GetVariable("_S_LibraryProjectName_S_")).Return("My.Football.Manager.Library");
                stubResolveContext.Stub(context => context.GetVariable("_S_ProjectDescription_S_")).Return("application for management of football teams.");

                var text = string.Format("The name of my console project is '_S_ConsoleProjectName_S_',{0}and the name of the corresponding library project is '_S_LibraryProjectName_S_'.{0}This is an _S_ProjectDescription_S_", Environment.NewLine);
                TestData.CreateTestTextFile(_sourceTestFile, Encoding.UTF8, text);
                var expected = string.Format("The name of my console project is 'My.Football.Manager',{0}and the name of the corresponding library project is 'My.Football.Manager.Library'.{0}This is an application for management of football teams.", Environment.NewLine);

                var target = testBooStrapper.Container.Resolve<IFileResolver>();
                target.Resolve(_sourceTestFile, _targetTestFile);
                var actual = TestData.ReadTestTextFile(_targetTestFile, Encoding.UTF8);
                Assert.AreEqual(expected, actual);
            }
        }

        [Test]
        public void ResolveFileAllVariablesKnownExceptOneTest()
        {
            using (var testBooStrapper = new TestBootStrapper(GetType()))
            {
                var stubResolveContext = testBooStrapper.Container.Resolve<IResolveContext>();
                stubResolveContext.Stub(context => context.GetVariable("_S_ConsoleProjectName_S_")).Return("My.Football.Manager");
                stubResolveContext.Stub(context => context.GetVariable("_S_LibraryProjectName_S_")).Return("My.Football.Manager.Library");
                stubResolveContext.Stub(context => context.GetVariable("_S_ProjectDescription_S_")).Return("application for management of football teams.");

                var text = string.Format("The name of my console project is '_S_ConsoleProjectName_S_',{0}and the name of the corresponding library project is '_S_LibraryProjectName_S_'.{0}This is an _S_ProjectDescription_S_ The following '_S_SomeUnknownVariable_S_' is unknown and should not be resolved, just leave it as is.", Environment.NewLine);
                TestData.CreateTestTextFile(_sourceTestFile, Encoding.UTF8, text);
                var expected = string.Format("The name of my console project is 'My.Football.Manager',{0}and the name of the corresponding library project is 'My.Football.Manager.Library'.{0}This is an application for management of football teams. The following '_S_SomeUnknownVariable_S_' is unknown and should not be resolved, just leave it as is.", Environment.NewLine);

                var target = testBooStrapper.Container.Resolve<IFileResolver>();
                target.Resolve(_sourceTestFile, _targetTestFile);
                var actual = TestData.ReadTestTextFile(_targetTestFile, Encoding.UTF8);
                Assert.AreEqual(expected, actual);
            }
        }

        [Test]
        public void ResolveFileBinaryUntouchedTest()
        {
            using (var testBooStrapper = new TestBootStrapper(GetType()))
            {
                var stubResolveContext = testBooStrapper.Container.Resolve<IResolveContext>();
                stubResolveContext.Stub(context => context.GetVariable("_S_ConsoleProjectName_S_")).Return("My.Football.Manager");
                stubResolveContext.Stub(context => context.GetVariable("_S_LibraryProjectName_S_")).Return("My.Football.Manager.Library");
                stubResolveContext.Stub(context => context.GetVariable("_S_ProjectDescription_S_")).Return("application for management of football teams.");

                //Try to resolve the current test dll and check that the result is an untouched
                var binaryFile = new FileInfo(typeof(NewResolveFileTests).Assembly.Location);
                File.Copy(binaryFile.FullName, _sourceTestFile, true);
                var target = testBooStrapper.Container.Resolve<IFileResolver>();
                target.Resolve(_sourceTestFile, _targetTestFile);
                Assert.IsTrue(_fileComparer.Compare(new FileInfo(_sourceTestFile), new FileInfo(_targetTestFile)) == CompareResult.Equal);
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

    //[TestFixture(Category = "UnitTests")]
    //public class ResolveFileTests
    //{
    //    private ILog _logger;
    //    private IResolveContext _stubResolveContext;
    //    private string _sourceTestFile;
    //    private FileEncoding _fileEncoding;
    //    private TextResolver _textResolver;
    //    private string _targetTestFile;
    //    private IFileComparer _fileComparer;
    //    private FileCopy _fileCopy;

    //    [SetUp]
    //    public void SetUp()
    //    {
    //        _logger = new ConsoleOutLogger(this.GetType().Name, LogLevel.All, true, false, false, "yyyy-MM-dd hh:mm:ss");
    //        _stubResolveContext = MockRepository.GenerateStub<IResolveContext>();
    //        _stubResolveContext.Stub(context => context.GetVariable("ConsoleProjectName")).Return("My.Football.Manager");
    //        _stubResolveContext.Stub(context => context.GetVariable("LibraryProjectName")).Return("My.Football.Manager.Library");
    //        _stubResolveContext.Stub(context => context.GetVariable("ProjectDescription")).Return("application for management of football teams.");
    //        _fileEncoding = new FileEncoding();
    //        _textResolver = new TextResolver(_stubResolveContext, _logger);
    //        _sourceTestFile = Path.GetTempFileName();
    //        _targetTestFile = Path.GetTempFileName();
    //        if (File.Exists(_sourceTestFile))
    //        {
    //            File.Delete(_sourceTestFile);
    //        }
    //        if (File.Exists(_targetTestFile))
    //        {
    //            File.Delete(_targetTestFile);
    //        }
    //        _fileComparer = new FileComparer(new FileTimeComparer(_logger), _logger);
    //        _fileCopy = new FileCopy();
    //    }

    //    [TearDown]
    //    public void TearDown()
    //    {
    //        if (File.Exists(_sourceTestFile))
    //        {
    //            File.Delete(_sourceTestFile);
    //        }
    //        if (File.Exists(_targetTestFile))
    //        {
    //            File.Delete(_targetTestFile);
    //        }
    //    }

    //    [Test]
    //    public void ResolveFileAllVariablesKnownTest()
    //    {
    //        var text = string.Format("The name of my console project is '_S_ConsoleProjectName_S_',{0}and the name of the corresponding library project is '_S_LibraryProjectName_S_'.{0}This is an _S_ProjectDescription_S_", Environment.NewLine);
    //        TestData.CreateTestTextFile(_sourceTestFile, Encoding.UTF8, text);
    //        var expected = string.Format("The name of my console project is 'My.Football.Manager',{0}and the name of the corresponding library project is 'My.Football.Manager.Library'.{0}This is an application for management of football teams.", Environment.NewLine);
    //        var target = new FileResolver(_fileEncoding, _textResolver, _fileCopy, _logger);
    //        target.Resolve(_sourceTestFile, _targetTestFile);
    //        var actual = TestData.ReadTestTextFile(_targetTestFile, Encoding.UTF8);
    //        Assert.AreEqual(expected, actual);
    //    }

    //    [Test]
    //    public void ResolveFileAllVariablesKnownExceptOneTest()
    //    {
    //        var text = string.Format("The name of my console project is '_S_ConsoleProjectName_S_',{0}and the name of the corresponding library project is '_S_LibraryProjectName_S_'.{0}This is an _S_ProjectDescription_S_ The following '_S_SomeUnknownVariable_S_' is unknown and should not be resolved, just leave it as is.", Environment.NewLine);
    //        TestData.CreateTestTextFile(_sourceTestFile, Encoding.UTF8, text);
    //        var expected = string.Format("The name of my console project is 'My.Football.Manager',{0}and the name of the corresponding library project is 'My.Football.Manager.Library'.{0}This is an application for management of football teams. The following '_S_SomeUnknownVariable_S_' is unknown and should not be resolved, just leave it as is.", Environment.NewLine);
    //        var target = new FileResolver(_fileEncoding, _textResolver, _fileCopy, _logger);
    //        target.Resolve(_sourceTestFile, _targetTestFile);
    //        var actual = TestData.ReadTestTextFile(_targetTestFile, Encoding.UTF8);
    //        Assert.AreEqual(expected, actual);
    //    }

    //    [Test]
    //    public void ResolveFileBinaryUntouchedTest()
    //    {
    //        //Try to resolve the current test dll and check that the resul is an untouched
    //        var binaryFile = new FileInfo(typeof(ResolveFileTests).Assembly.Location);
    //        File.Copy(binaryFile.FullName, _sourceTestFile, true);
    //        var target = new FileResolver(_fileEncoding, _textResolver, _fileCopy, _logger);
    //        target.Resolve(_sourceTestFile, _targetTestFile);
    //        Assert.IsTrue(_fileComparer.Compare(new FileInfo(_sourceTestFile), new FileInfo(_targetTestFile)) == CompareResult.Equal);
    //    }
    //}
}