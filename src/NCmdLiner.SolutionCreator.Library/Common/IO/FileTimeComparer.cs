using System;
using System.IO;
using Common.Logging;

namespace NCmdLiner.SolutionCreator.Library.Common.IO
{
    public class FileTimeComparer : IFileTimeComparer
    {
        private readonly ILog _logger;

        public FileTimeComparer(ILog logger)
        {
            _logger = logger;
        }

        public CompareResult Compare(FileInfo file1, FileInfo file2)
        {
            if (file1 == null) throw new ArgumentNullException("file1");
            if (file2 == null) throw new ArgumentNullException("file2");

            if (file1.CreationTimeUtc != file2.CreationTimeUtc)
            {
                if (_logger.IsDebugEnabled) _logger.DebugFormat("Creation times are not equal. '{0}' != '{1}'", file1.FullName, file2.FullName);
                return CompareResult.NotEqual;
            }
            if (file1.LastWriteTimeUtc != file2.LastWriteTimeUtc)
            {
                if (_logger.IsDebugEnabled) _logger.DebugFormat("Last write times are not equal. '{0}' != '{1}'", file1.FullName, file2.FullName);
                return CompareResult.NotEqual;
            }
            if (file1.LastAccessTimeUtc != file2.LastAccessTimeUtc)
            {
                if (_logger.IsDebugEnabled) _logger.DebugFormat("Last access times are not equal. '{0}' != '{1}'", file1.FullName, file2.FullName);
                return CompareResult.NotEqual;
            }
            if (_logger.IsDebugEnabled) _logger.DebugFormat("Time stamps are equal. '{0}' == '{1}'", file1.FullName, file2.FullName);
            return CompareResult.Equal;

        }
    }
}