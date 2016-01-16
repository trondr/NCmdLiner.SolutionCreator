namespace NCmdLiner.SolutionCreator.Library.Views
{
    public interface ISolutionInfoWindowFactory
    {
        SolutionInfoWindow GetSolutionInfoWindow();

        void Release(SolutionInfoWindow solutionInfoWindow);
    }
}