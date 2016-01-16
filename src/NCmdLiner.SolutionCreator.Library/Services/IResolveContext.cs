namespace NCmdLiner.SolutionCreator.Library.Services
{
    public interface IResolveContext
    {
        void AddVariable(string name, string value);

        string GetVariable(string name);
    }
}