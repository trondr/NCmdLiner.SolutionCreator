using System.IO;
using System.Text;
using Common.Logging;
using Common.Logging.Simple;
using NCmdLiner.SolutionCreator.Library.Common.IO;
using NUnit.Framework;

namespace NCmdLiner.SolutionCreator.Library.Tests.UnitTests
{
    [TestFixture(Category = "UnitTests")]
    public class FileEncodingTests
    {
        private ILog _logger;
        private string _fileName;

        [SetUp]
        public void SetUp()
        {
            _logger = new ConsoleOutLogger(this.GetType().Name, LogLevel.All, true, false, false, "yyyy-MM-dd hh:mm:ss");
            _fileName = Path.GetTempFileName();
            if (File.Exists(_fileName))
            {
                File.Delete(_fileName);
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_fileName))
            {
                File.Delete(_fileName);
            }
        }

        [Test]
        public void GetFileEncodingUtf8Test()
        {
            var expected = Encoding.UTF8;
            TestData.CreateTestTextFile(_fileName, expected, "This is a test string to write into the file.");
            var target = new FileEncoding();
            var actual = target.GetEncoding(_fileName);            
            Assert.AreEqual(expected,actual);
        }

        [Test]
        public void GetFileEncodingDefaultTest()
        {
            var expected = Encoding.Default;
            TestData.CreateTestTextFile(_fileName, expected, "This is a test string to write into the file.");
            var target = new FileEncoding();
            var actual = target.GetEncoding(_fileName);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetFileEncodingUtf7Test()
        {
            var expected = Encoding.UTF7;
            TestData.CreateTestTextFile(_fileName, expected, "This is a test string to write into the file. Does it support זרו?");
            var target = new FileEncoding();
            var actual = target.GetEncoding(_fileName);
            Assert.AreEqual(Encoding.Default, actual); //UTF7 does not result in a BOM so defaut is returned in this test
        }

        [Test]
        public void GetFileEncodingUtf32Test()
        {
            var expected = Encoding.UTF32;
            TestData.CreateTestTextFile(_fileName, expected, "This is a test string to write into the file. Does it support זרו?");
            var target = new FileEncoding();
            var actual = target.GetEncoding(_fileName);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetFileEncodingUnicodeTest()
        {
            var expected = Encoding.Unicode;
            TestData.CreateTestTextFile(_fileName, expected, "This is a test string to write into the file. Does it support זרו?");
            var target = new FileEncoding();
            var actual = target.GetEncoding(_fileName);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void GetFileEncodingBigEndianUnicodeTest()
        {
            var expected = Encoding.BigEndianUnicode;
            TestData.CreateTestTextFile(_fileName, expected, "This is a test string to write into the file. Does it support זרו?");
            var target = new FileEncoding();
            var actual = target.GetEncoding(_fileName);
            Assert.AreEqual(expected, actual);
        }

        
    }
}