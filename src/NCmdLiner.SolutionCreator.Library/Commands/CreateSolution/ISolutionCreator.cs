using NCmdLiner.SolutionCreator.Library.Model;

namespace NCmdLiner.SolutionCreator.Library.Commands.CreateSolution
{
    public interface ISolutionCreator
    {
        int Create(string targetRootFolder, IConsoleApplicationInfo consoleApplicationInfo);
    }
}
