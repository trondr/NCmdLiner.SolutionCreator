namespace NCmdLiner.SolutionCreator.Library.Services
{
    public interface IFolderResolver
    {
        void Resolve(string sourceFolder, string targetFolder);
    }
}
