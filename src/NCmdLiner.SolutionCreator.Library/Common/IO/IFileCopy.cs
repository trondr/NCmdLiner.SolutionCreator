namespace NCmdLiner.SolutionCreator.Library.Common.IO
{
    public interface IFileCopy
    {
        void Copy(string file1, string file2, bool overwrite);
    }
}
