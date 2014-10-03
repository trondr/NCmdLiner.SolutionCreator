using System;
using System.IO;

namespace NCmdLiner.SolutionCreator.Library.Common.IO
{
    public class FileDataComparer : IFileDataComparer
    {
        public CompareResult Compare(string file1, string file2)
        {
            if (file1 == null) throw new ArgumentNullException("file1");
            if (file2 == null) throw new ArgumentNullException("file2");

            if (!File.Exists(file1)) return CompareResult.NotEqual;
            if (!File.Exists(file2)) return CompareResult.NotEqual;

            var fileInfo1 = new FileInfo(file1);
            var fileInfo2 = new FileInfo(file2);

            if (fileInfo1.Length != fileInfo2.Length)
            {
                return CompareResult.NotEqual;
            }

            using (var fileStream1 = fileInfo1.OpenRead())
            {
                using (var fileStream2 = fileInfo2.OpenRead())
                {
                    for(var i = 0; i < fileInfo1.Length; i++)
                    {
                        var b1 = fileStream1.ReadByte();
                        var b2 = fileStream2.ReadByte();
                        if (b1 != b2)
                        {
                            return CompareResult.NotEqual;
                        }
                    }
                }
            }
            return CompareResult.Equal;
        }
    }
}