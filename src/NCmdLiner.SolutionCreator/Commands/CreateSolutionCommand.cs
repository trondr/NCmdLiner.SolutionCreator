using System.Windows.Forms;
using Common.Logging;
using NCmdLiner.Attributes;
using NCmdLiner.SolutionCreator.Library.Common;
using NCmdLiner.SolutionCreator.Library.Views;

namespace NCmdLiner.SolutionCreator.Commands
{
    public class CreateSolutionCommand: CommandsBase
    {
        private readonly MainWindow _mainWindow;
        private readonly ILog _logger;

        public CreateSolutionCommand(MainWindow mainWindow,ILog logger)
        {
            _mainWindow = mainWindow;
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
            var application = new System.Windows.Application();
            application.Run(_mainWindow);
            _logger.Info("End CreateSolution.");
            return returnValue;
        }
    }
}
