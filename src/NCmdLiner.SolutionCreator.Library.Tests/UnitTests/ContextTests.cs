using System;
using Common.Logging;
using Common.Logging.Simple;
using NCmdLiner.SolutionCreator.Library.Services;
using NUnit.Framework;
using Rhino.Mocks;

namespace NCmdLiner.SolutionCreator.Library.Tests.UnitTests
{
    [TestFixture(Category = "UnitTests")]
    public class ContextTests
    {
        private ILog _logger;
        private IGuidGeneator _stubGuidGenerator;
        private string _guid1;
        private string _guid2;        

        [SetUp]
        public void SetUp()
        {
            _logger = new ConsoleOutLogger(this.GetType().Name, LogLevel.All, true, false, false, "yyyy-MM-dd hh:mm:ss");

            _guid1 = Guid.NewGuid().ToString(); 
            Console.WriteLine("Guid1="+_guid1);
            
            _guid2 = Guid.NewGuid().ToString(); 
            Console.WriteLine("Guid2=" + _guid2);            
            
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void ContextAddVariableGetVariableTest()
        {
            _stubGuidGenerator = MockRepository.GenerateStub<IGuidGeneator>();
            _stubGuidGenerator.Stub(geneator => geneator.GetNewGuid()).Repeat.Never();

            var target = new Context(_stubGuidGenerator, _logger);
            const string variableName = "SomeVariableName";
            const string expected = "SomeValue";
            var actual = target.GetVariable(variableName);
            Assert.AreEqual(null, actual);
            target.AddVariable(variableName, expected);
            actual = target.GetVariable(variableName);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ContextGuidTest()
        {
            _stubGuidGenerator = MockRepository.GenerateStub<IGuidGeneator>();
            _stubGuidGenerator.Stub(geneator => geneator.GetNewGuid()).Return(_guid1).Repeat.Once();
            _stubGuidGenerator.Stub(geneator => geneator.GetNewGuid()).Return(_guid2).Repeat.Once();
            

            var target = new Context(_stubGuidGenerator, _logger);
            var actual = target.GetVariable("Guid1");
            var expected = _guid1;
            Assert.AreEqual(expected, actual);
            actual = target.GetVariable("Guid2");
            expected = _guid2;
            Assert.AreEqual(expected, actual);
            actual = target.GetVariable("Guid1");
            expected = _guid1;
            Assert.AreEqual(expected, actual);
            actual = target.GetVariable("Guid2");
            expected = _guid2;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ContextInvalidGuidNameTest()
        {
            _stubGuidGenerator = MockRepository.GenerateStub<IGuidGeneator>();
            _stubGuidGenerator.Stub(geneator => geneator.GetNewGuid()).Repeat.Never();
            
            var target = new Context(_stubGuidGenerator, _logger);
            var actual = target.GetVariable("Guidd1");
            string expected = null;
            Assert.AreEqual(expected, actual);
            actual = target.GetVariable("Guidd2");
            expected = null;
            Assert.AreEqual(expected, actual);
            actual = target.GetVariable("Guidd1");
            expected = null;
            Assert.AreEqual(expected, actual);
            actual = target.GetVariable("Guidd2");
            expected = null;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ContextInvalidLowerCaseGuidNameTest()
        {
            _stubGuidGenerator = MockRepository.GenerateStub<IGuidGeneator>();
            _stubGuidGenerator.Stub(geneator => geneator.GetNewGuid()).Repeat.Never();

            var target = new Context(_stubGuidGenerator, _logger);
            var actual = target.GetVariable("guid1");
            string expected = null;
            Assert.AreEqual(expected, actual);
            actual = target.GetVariable("guid2");
            expected = null;
            Assert.AreEqual(expected, actual);
            actual = target.GetVariable("guid1");
            expected = null;
            Assert.AreEqual(expected, actual);
            actual = target.GetVariable("guid2");
            expected = null;
            Assert.AreEqual(expected, actual);
        }
    }
}