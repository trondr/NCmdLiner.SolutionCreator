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
    public class ResolveFileTests
    {
        private ILog _logger;
        private IContext _stubContext;
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
            _stubContext = MockRepository.GenerateStub<IContext>();
            _stubContext.Stub(context => context.GetVariable("ConsoleProjectName")).Return("My.Football.Manager");
            _stubContext.Stub(context => context.GetVariable("LibraryProjectName")).Return("My.Football.Manager.Library");
            _stubContext.Stub(context => context.GetVariable("ProjectDescription")).Return("application for management of football teams.");
            _fileEncoding = new FileEncoding();
            _textResolver = new TextResolver(_stubContext, _logger);
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
            var text = string.Format("The name of my console project is '_S_ConsoleProjectName_S_',{0}and the name of the corresponding library project is '_S_LibraryProjectName_S_'.{0}This is an _S_ProjectDescription_S_", Environment.NewLine);
            TestData.CreateTestTextFile(_sourceTestFile,Encoding.UTF8, text);
            var expected = string.Format("The name of my console project is 'My.Football.Manager',{0}and the name of the corresponding library project is 'My.Football.Manager.Library'.{0}This is an application for management of football teams.", Environment.NewLine);
            var target = new FileResolver(_fileEncoding, _textResolver, _fileCopy, _logger);
            target.Resolve(_sourceTestFile,_targetTestFile);
            var actual = TestData.ReadTestTextFile(_targetTestFile,Encoding.UTF8);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolveFileAllVariablesKnownExceptOneTest()
        {
            var text = string.Format("The name of my console project is '_S_ConsoleProjectName_S_',{0}and the name of the corresponding library project is '_S_LibraryProjectName_S_'.{0}This is an _S_ProjectDescription_S_ The following '_S_SomeUnknownVariable_S_' is unknown and should not be resolved, just leave it as is.", Environment.NewLine);
            TestData.CreateTestTextFile(_sourceTestFile, Encoding.UTF8, text);
            var expected = string.Format("The name of my console project is 'My.Football.Manager',{0}and the name of the corresponding library project is 'My.Football.Manager.Library'.{0}This is an application for management of football teams. The following '_S_SomeUnknownVariable_S_' is unknown and should not be resolved, just leave it as is.", Environment.NewLine);
            var target = new FileResolver(_fileEncoding, _textResolver, _fileCopy, _logger);
            target.Resolve(_sourceTestFile, _targetTestFile);
            var actual = TestData.ReadTestTextFile(_targetTestFile, Encoding.UTF8);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolveFileBinaryUntouchedTest()
        {
            //Try to resolve the current test dll and check that the resul is an untouched
            var binaryFile = new FileInfo(typeof (ResolveFileTests).Assembly.Location);
            File.Copy(binaryFile.FullName,_sourceTestFile, true);
            var target = new FileResolver(_fileEncoding, _textResolver, _fileCopy, _logger);
            target.Resolve(_sourceTestFile, _targetTestFile);
            Assert.IsTrue(_fileComparer.Compare(new FileInfo(_sourceTestFile),new FileInfo(_targetTestFile)) == CompareResult.Equal);
        }
    }
}