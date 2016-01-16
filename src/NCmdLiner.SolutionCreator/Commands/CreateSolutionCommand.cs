using Common.Logging;
using NCmdLiner.Attributes;
using NCmdLiner.SolutionCreator.Library.Commands.CreateSolution;
using NCmdLiner.SolutionCreator.Library.Common;
using NCmdLiner.SolutionCreator.Library.Model;

namespace NCmdLiner.SolutionCreator.Commands
{
    public class CreateSolutionCommand: CommandDefinition
    {
        private readonly ISolutionCreatorCommandProvider _solutionCreatorCommandProvider;
        private readonly ILog _logger;

        public CreateSolutionCommand(ISolutionCreatorCommandProvider solutionCreatorCommandProvider, ILog logger)
        {
            _solutionCreatorCommandProvider = solutionCreatorCommandProvider;
            _logger = logger;
        }

        [Command(Description = "Create new solution in target root folder. User will be shown a dialog where some basic information must be entered. New solution will be created in the target root folder based on the entered basic information.")]
        public int CreateSolution(
            [RequiredCommandParameter(Description = "Root folder where the new solution will be created.", AlternativeName = "rf", ExampleValue = @"c:\Dev")]
            string targetRootFolder
            )
        {
            var returnValue = 0;
            _logger.Info("Start CreateSolution...");
            IConsoleApplicationInfo consoleApplicationInfo = new ConsoleApplicationInfo();
            returnValue = _solutionCreatorCommandProvider.CreateSolution(targetRootFolder, consoleApplicationInfo);
            _logger.Info("End CreateSolution.");
            return returnValue;
        }
    }
}
