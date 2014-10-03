using System;
using System.IO;
using Common.Logging;

namespace NCmdLiner.SolutionCreator.Library.Common.IO
{
    public class FileComparer : IFileComparer
    {
        private readonly IFileTimeComparer _fileTimeComparer;
        private readonly ILog _logger;

        public FileComparer(IFileTimeComparer fileTimeComparer,ILog logger)
        {
            _fileTimeComparer = fileTimeComparer;
            _logger = logger;
        }

        public CompareResult Compare(FileInfo file1, FileInfo file2)
        {
            if (file1 == null) throw new ArgumentNullException("file1");
            if (file2 == null) throw new ArgumentNullException("file2");

            if (!file1.Exists) return CompareResult.NotEqual;
            if (!file2.Exists) return CompareResult.NotEqual;

            if (file1.Length != file2.Length)
            {
                if (_logger.IsDebugEnabled) _logger.DebugFormat("File length is not equal. '{0}' != '{1}'", file1.FullName, file2.FullName);
                return CompareResult.NotEqual;
            }

            if (_fileTimeComparer.Compare(file1, file2) == CompareResult.NotEqual)
            {
                if (_logger.IsDebugEnabled) _logger.DebugFormat("File times are not equal. '{0}' != '{1}'", file1.FullName, file2.FullName);
                return CompareResult.NotEqual;
            }

            using (var fileStream1 = file1.OpenRead())
            {
                using (var fileStream2 = file2.OpenRead())
                {
                    for (var i = 0; i < file1.Length; i++)
                    {
                        var b1 = fileStream1.ReadByte();
                        var b2 = fileStream2.ReadByte();
                        if (b1 != b2)
                        {
                            if (_logger.IsDebugEnabled) _logger.DebugFormat("File data is not equal. '{0}' != '{1}'", file1.FullName, file2.FullName);
                            return CompareResult.NotEqual;
                        }
                    }
                }
            }
            if (_logger.IsDebugEnabled) _logger.DebugFormat("Files are equal. '{0}' == '{1}'", file1.FullName, file2.FullName);
            return CompareResult.Equal;
        }
    }
}