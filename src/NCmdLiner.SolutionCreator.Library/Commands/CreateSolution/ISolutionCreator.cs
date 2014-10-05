using System.Windows.Documents;

namespace NCmdLiner.SolutionCreator.Library.Commands.CreateSolution
{
    public interface ISolutionCreator
    {
        int Create(string targetRootFolder);
    }
}
