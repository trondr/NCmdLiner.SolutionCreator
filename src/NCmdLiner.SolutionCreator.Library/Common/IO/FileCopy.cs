using System.IO;

namespace NCmdLiner.SolutionCreator.Library.Common.IO
{
    public class FileCopy : IFileCopy
    {
        public void Copy(string file1, string file2, bool overwrite)
        {
            File.Copy(file1,file2,overwrite);
            File.SetLastWriteTime(file2,File.GetLastWriteTime(file1));
            File.SetLastAccessTime(file2, File.GetLastAccessTime(file1));
            File.SetCreationTime(file2, File.GetCreationTime(file1));
        }
    }
}