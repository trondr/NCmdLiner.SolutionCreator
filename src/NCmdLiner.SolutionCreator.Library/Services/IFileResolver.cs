namespace NCmdLiner.SolutionCreator.Library.Services
{
    public interface IFileResolver
    {
        void Resolve(string sourceFile, string targetFile);
    }
}