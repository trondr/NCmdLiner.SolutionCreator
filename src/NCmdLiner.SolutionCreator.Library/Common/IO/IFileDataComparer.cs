namespace NCmdLiner.SolutionCreator.Library.Common.IO
{
    public interface IFileDataComparer
    {
        CompareResult Compare(string file1, string file2);
    }
}
