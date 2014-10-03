using System;
using System.IO;
using System.Text;
using Common.Logging;
using Common.Logging.Simple;
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
        private IContext _stubContext;        
        private FileEncoding _fileEncoding;
        private TextResolver _textResolver;        
        private IFileComparer _fileComparer;
        private FileCopy _fileCopy;
        private FileResolver _fileResolver;
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
            _stubContext = MockRepository.GenerateStub<IContext>();
            _stubContext.Stub(context => context.GetVariable("ConsoleProjectName")).Return("My.Football.Manager");
            _stubContext.Stub(context => context.GetVariable("LibraryProjectName")).Return("My.Football.Manager.Library");
            _stubContext.Stub(context => context.GetVariable("ProjectDescription")).Return("application for management of football teams.");
            _fileEncoding = new FileEncoding();
            _textResolver = new TextResolver(_stubContext, _logger);
            _fileComparer = new FileComparer(new FileTimeComparer(_logger),_logger );
            _fileCopy = new FileCopy();
            _fileResolver = new FileResolver(_fileEncoding, _textResolver, _fileCopy, _logger);
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
            _targetSubTestFolderFile = _textResolver.Resolve(_sourceSubTestFolderFile).Replace(_sourceTestFolder,_targetTestFolder);
            _targetSubSubTestFolderFile = _textResolver.Resolve(_sourceSubSubTestFolderFile).Replace(_sourceTestFolder, _targetTestFolder);
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
            Assert.IsTrue(File.Exists(_sourceSubTestFolderFile), "File does not exist:" + _sourceSubTestFolderFile);
            Assert.IsTrue(File.Exists(_sourceSubSubTestFolderFile), "File does not exist:" + _sourceSubSubTestFolderFile);
            var target = new FolderResolver(_textResolver,_fileResolver,_logger);
            target.Resolve(_sourceTestFolder, _targetTestFolder);
            Assert.IsTrue(File.Exists(_targetSubTestFolderFile), "File does not exist:" + _targetSubTestFolderFile);
            Assert.IsTrue(File.Exists(_targetSubSubTestFolderFile), "File does not exist:" + _targetSubSubTestFolderFile);
        }
    }
}