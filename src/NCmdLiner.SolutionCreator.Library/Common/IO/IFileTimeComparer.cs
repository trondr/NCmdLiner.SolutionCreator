using System.IO;

namespace NCmdLiner.SolutionCreator.Library.Common.IO
{
    public interface IFileTimeComparer
    {
        CompareResult Compare(FileInfo file1, FileInfo file2);
    }
}