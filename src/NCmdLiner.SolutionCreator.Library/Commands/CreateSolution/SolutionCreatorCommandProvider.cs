using NCmdLiner.SolutionCreator.Library.Common;
using NCmdLiner.SolutionCreator.Library.Model;
using NCmdLiner.SolutionCreator.Library.Views;

namespace NCmdLiner.SolutionCreator.Library.Commands.CreateSolution
{
    public class SolutionCreatorCommandProvider : CommandProvider, ISolutionCreatorCommandProvider
    {
        private readonly ICreateSolutionApplication _createSolutionApplication;

        public SolutionCreatorCommandProvider(ICreateSolutionApplication createSolutionApplication)
        {
            _createSolutionApplication = createSolutionApplication;
        }

        public int CreateSolution(string targetRootFolder, IConsoleApplicationInfo consoleApplicationInfo)
        {            
            _createSolutionApplication.InitializeAndRun(targetRootFolder);
            return _createSolutionApplication.ExitCode;
        }
    }
}