namespace NCmdLiner.SolutionCreator.Library.Commands.CreateSolution
{
    public interface ISolutionCreatorCommandProviderFactory
    {
        ISolutionCreatorCommandProvider GetSolutionCreatorCommandProvider();

        void Release(ISolutionCreatorCommandProvider solutionCreatorCommandProvider);

    }
}