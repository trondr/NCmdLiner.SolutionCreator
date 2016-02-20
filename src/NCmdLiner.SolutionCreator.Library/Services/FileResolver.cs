using System.IO;
using System.Text;
using Common.Logging;
using NCmdLiner.SolutionCreator.Library.Common.IO;

namespace NCmdLiner.SolutionCreator.Library.Services
{
    public class FileResolver : IFileResolver
    {
        private readonly IFileEncoding _fileEncoding;
        private readonly ITextResolver _textResolver;
        private readonly IFileCopy _fileCopy;
        private readonly ICheckFileExtentions _checkFileExtentions;
        private readonly ILog _logger;

        public FileResolver(IFileEncoding fileEncoding, ITextResolver textResolver, IFileCopy fileCopy, ICheckFileExtentions checkFileExtentions, ILog logger)
        {
            _fileEncoding = fileEncoding;
            _textResolver = textResolver;
            _fileCopy = fileCopy;
            _checkFileExtentions = checkFileExtentions;
            _logger = logger;
        }

        public void Resolve(string sourceFile, string targetFile)
        {
            var fileExtension = Path.GetExtension(sourceFile)?.ToLower();
            if (_checkFileExtentions.IsBinaryFileExtension(fileExtension))
            {
                CopyFile(sourceFile, targetFile);
            }
            else
            {
                var fileEncoding = _fileEncoding.GetEncoding(sourceFile);
                var unresolvedText = ReadAllTextFromFile(sourceFile, fileEncoding);
                var resolvedText = _textResolver.Resolve(unresolvedText);
                if (resolvedText != unresolvedText)
                {
                    WriteTextToFile(targetFile, resolvedText, fileEncoding);
                }
                else
                {
                    _logger.DebugFormat("The unresolved and resolved data is equal, copy file instead of resolving file.");
                    CopyFile(sourceFile, targetFile);
                }
            }
        }

        private void WriteTextToFile(string targetFile, string resolvedText, Encoding fileEncoding)
        {
            var temporaryTargetFile = Path.GetTempFileName();
            using (var sw = new StreamWriter(temporaryTargetFile, false, fileEncoding))
            {
                sw.Write(resolvedText);
            }
            if (File.Exists(targetFile))
            {
                File.Delete(targetFile);
            }
            File.Move(temporaryTargetFile, targetFile);
        }

        private void CopyFile(string sourceFile, string targetFile)
        {
            _logger.DebugFormat("Copying file '{0}'->'{1}'...", sourceFile, targetFile);
            if (sourceFile != targetFile)
            {
                _fileCopy.Copy(sourceFile, targetFile, false);
            }
            else
            {
                _logger.DebugFormat("Nothing to copy, source file and to target file is the same. '{0}'=='{1}'", sourceFile, targetFile);
            }
        }

        private string ReadAllTextFromFile(string fileName, Encoding fileEncoding)
        {
            using (var sr = new StreamReader(fileName, fileEncoding))
            {
                return sr.ReadToEnd();
            }
        }
    }
}