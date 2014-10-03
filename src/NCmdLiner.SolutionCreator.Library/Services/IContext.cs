namespace NCmdLiner.SolutionCreator.Library.Services
{
    public interface IContext
    {
        void AddVariable(string name, string value);

        string GetVariable(string name);
    }
}