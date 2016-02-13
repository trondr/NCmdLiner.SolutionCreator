using System.Globalization;

namespace NCmdLiner.SolutionCreator.Library.Views
{
    public interface ICreateSolutionApplication
    {
        void InitializeAndRun(string targetRootFolder);

        int ExitCode {get;set; }
    }
}