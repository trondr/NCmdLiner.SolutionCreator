using NCmdLiner.SolutionCreator.Library.Model;

namespace NCmdLiner.SolutionCreator.Library.Commands.CreateSolution
{
    public interface IClientServiceApplicationSolutionCreator
    {
        int Create(string targetRootFolder, IClientServiceApplicationInfo clientServiceApplicationInfo);
    }
}