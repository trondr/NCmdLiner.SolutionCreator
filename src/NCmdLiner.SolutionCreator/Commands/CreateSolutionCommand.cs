using System.Windows.Forms;
using Common.Logging;
using NCmdLiner.Attributes;
using NCmdLiner.SolutionCreator.Library.Common;

namespace NCmdLiner.SolutionCreator.Commands
{
    public class CreateSolutionCommand: CommandsBase
    {
        private readonly ILog _logger;

        public CreateSolutionCommand(ILog logger)
        {
            _logger = logger;
        }

        [Command(Description = "Create new solution in target root folder.")]
        public int CreateSolution(
            [RequiredCommandParameter(Description = "Root folder where the new solution will be created.",AlternativeName = "rf", ExampleValue = @"c:\Dev")]
            string targetRootFolder
            )
        {
            var returnValue = 0;
            _logger.Info("Start CreateSolution...");
            MessageBox.Show("Dummy create solution dialog.");
            _logger.Info("End CreateSolution.");
            return returnValue;
        }
    }
}
