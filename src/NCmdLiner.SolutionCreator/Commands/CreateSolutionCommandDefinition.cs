using Common.Logging;
using NCmdLiner.Attributes;
using NCmdLiner.SolutionCreator.Library.Commands.CreateSolution;
using NCmdLiner.SolutionCreator.Library.Common;
using NCmdLiner.SolutionCreator.Library.Model;

namespace NCmdLiner.SolutionCreator.Commands
{
    public class CreateSolutionCommandDefinition: CommandDefinition
    {
        private readonly ISolutionCreatorCommandProviderFactory _solutionCreatorCommandProviderFactory;
        private readonly ILog _logger;

        public CreateSolutionCommandDefinition(ISolutionCreatorCommandProviderFactory solutionCreatorCommandProviderFactory, ILog logger)
        {
            _solutionCreatorCommandProviderFactory = solutionCreatorCommandProviderFactory;
            _logger = logger;
        }

        [Command(Description = "Create new solution in target root folder. User will be shown a dialog where some basic information must be entered. New solution will be created in the target root folder based on the entered basic information.")]
        public int CreateSolution(
            [RequiredCommandParameter(Description = "Root folder where the new solution will be created.", AlternativeName = "rf", ExampleValue = @"c:\Dev")]
            string targetRootFolder
            )
        {
            int returnValue;
            _logger.Info("Start CreateSolution...");
            ISolutionCreatorCommandProvider solutionCreatorCommandProvider = null;
            try
            {
                solutionCreatorCommandProvider = _solutionCreatorCommandProviderFactory.GetSolutionCreatorCommandProvider();
                returnValue = solutionCreatorCommandProvider.CreateSolution(targetRootFolder, new ConsoleApplicationInfo());
            }
            finally
            {
                _solutionCreatorCommandProviderFactory.Release(solutionCreatorCommandProvider);
            }            
            _logger.Info("End CreateSolution.");
            return returnValue;
        }
    }
}
