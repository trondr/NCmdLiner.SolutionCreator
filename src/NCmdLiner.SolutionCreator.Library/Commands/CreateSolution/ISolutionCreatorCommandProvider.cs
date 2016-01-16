using NCmdLiner.SolutionCreator.Library.Model;

namespace NCmdLiner.SolutionCreator.Library.Commands.CreateSolution
{
    public interface ISolutionCreatorCommandProvider
    {
        int CreateSolution(string targetRootFolder, IConsoleApplicationInfo consoleApplicationInfo);
    }
}
