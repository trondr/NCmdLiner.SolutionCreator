using System.IO;
using System.Text;
using Common.Logging;
using Common.Logging.Simple;
using NCmdLiner.SolutionCreator.Library.Common;
using NCmdLiner.SolutionCreator.Library.Common.IO;
using NCmdLiner.SolutionCreator.Library.Services;
using NUnit.Framework;

namespace NCmdLiner.SolutionCreator.Library.Tests.UnitTests
{
    [TestFixture(Category = "UnitTests")]
    public class TemplatePathTests  
    {
        private ILog _logger;        
        private string _currentDirectory;

        public TemplatePathTests()
        {
            
        }

        [SetUp]
        public void SetUp()
        {
            _logger = new ConsoleOutLogger(this.GetType().Name, LogLevel.All, true, false, false, "yyyy-MM-dd hh:mm:ss");
            var directoryInfo = new FileInfo(typeof(TemplatePath).Assembly.Location).Directory;
            if (directoryInfo != null)
                _currentDirectory = directoryInfo.FullName;
        }

        [TearDown]
        public void TearDown()
        {
            
        }

        [Test]
        public void GetFileEncodingUtf8Test()
        {
            var target = new TemplatePath();
            var expected = Path.Combine(_currentDirectory, "Templates");
            var actual = target.GetFullPath(@".\Templates");
            Assert.AreEqual(expected, actual);
        }
    }
}