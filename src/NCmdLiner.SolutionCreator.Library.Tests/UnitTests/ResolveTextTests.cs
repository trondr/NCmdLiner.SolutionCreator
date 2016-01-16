using Common.Logging;
using Common.Logging.Simple;
using NCmdLiner.SolutionCreator.Library.Services;
using NUnit.Framework;
using Rhino.Mocks;

namespace NCmdLiner.SolutionCreator.Library.Tests.UnitTests
{
    [TestFixture(Category = "UnitTests")]
    public class ResolveTextTests
    {
        private ILog _logger;
        private IResolveContext _stubResolveContext;

        [SetUp]
        public void SetUp()
        {
            _logger = new ConsoleOutLogger(this.GetType().Name, LogLevel.All, true, false, false, "yyyy-MM-dd hh:mm:ss");
            _stubResolveContext = MockRepository.GenerateStub<IResolveContext>();            
            _stubResolveContext.Stub(context => context.GetVariable("ConsoleProjectName")).Return("My.Football.Manager");
            _stubResolveContext.Stub(context => context.GetVariable("LibraryProjectName")).Return("My.Football.Manager.Library");
            _stubResolveContext.Stub(context => context.GetVariable("ProjectDescription")).Return("application for management of football teams.");
            _stubResolveContext.Stub(context => context.GetVariable("ECD7A685-EDCC-474C-AD38-000000000001")).Return("E1685BCA-21CE-4D7F-B030-90CC78502E76");
            _stubResolveContext.Stub(context => context.GetVariable("ECD7A685-EDCC-474C-AD38-000000000002")).Return("752D06E5-5B1F-4FC0-9AFE-E81ED61818F7");
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void ResolveTextAllVariablesKnownTest()
        {
            const string text = "The name of my console project is '_S_ConsoleProjectName_S_', and the name of the corresponding library project is '_S_LibraryProjectName_S_'. This is an _S_ProjectDescription_S_";
            const string expected = "The name of my console project is 'My.Football.Manager', and the name of the corresponding library project is 'My.Football.Manager.Library'. This is an application for management of football teams.";
            var target = new TextResolver(_stubResolveContext,_logger);
            var actual = target.Resolve(text);
            Assert.AreEqual(expected, actual);            
        }

        [Test]
        public void ResolveTextAllVariablesKnownExceptOneTest()
        {
            const string text = "The name of my console project is '_S_ConsoleProjectName_S_', and the name of the corresponding library project is '_S_LibraryProjectName_S_'. This is an _S_ProjectDescription_S_ The following '_S_SomeUnknownVariable_S_' is unknown and should not be resolved, just leave it as is.";
            const string expected = "The name of my console project is 'My.Football.Manager', and the name of the corresponding library project is 'My.Football.Manager.Library'. This is an application for management of football teams. The following '_S_SomeUnknownVariable_S_' is unknown and should not be resolved, just leave it as is.";
            var target = new TextResolver(_stubResolveContext, _logger);
            var actual = target.Resolve(text);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolvePredefinedGuidTest()
        {
            const string text = "Some text with the special NCmdLiner.SolutionCreator guid {ECD7A685-EDCC-474C-AD38-000000000001} and {ECD7A685-EDCC-474C-AD38-000000000002} should be translated to a new unique guid.";
            const string expected = "Some text with the special NCmdLiner.SolutionCreator guid {E1685BCA-21CE-4D7F-B030-90CC78502E76} and {752D06E5-5B1F-4FC0-9AFE-E81ED61818F7} should be translated to a new unique guid.";
            var target = new TextResolver(_stubResolveContext, _logger);
            var actual = target.Resolve(text);
            Assert.AreEqual(expected, actual);
        }
    }
}
