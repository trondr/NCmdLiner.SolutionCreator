using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Documents;
using Common.Logging;
using NCmdLiner.SolutionCreator.Library.Common;
using NCmdLiner.SolutionCreator.Library.Common.IO;

namespace NCmdLiner.SolutionCreator.Library.Services
{
    public class FileResolver : IFileResolver
    {
        private readonly IFileEncoding _fileEncoding;
        private readonly ITextResolver _textResolver;
        private readonly IFileCopy _fileCopy;
        private readonly ILog _logger;

        public FileResolver(IFileEncoding fileEncoding, ITextResolver textResolver, IFileCopy fileCopy, ILog logger)
        {
            _fileEncoding = fileEncoding;
            _textResolver = textResolver;
            _fileCopy = fileCopy;
            _logger = logger;
        }

        public void Resolve(string sourceFile, string targetFile)
        {
            ToDo.Implement(ToDoPriority.Critical, "trondr","Implement resolve of file");
            var fileEncoding = _fileEncoding.GetEncoding(sourceFile);
            string resolvedText;
            var copyAction = CopyAction.Resolve;
            using (var sr = new StreamReader(sourceFile, fileEncoding))
            {
                var unresolvedText = sr.ReadToEnd();
                resolvedText = _textResolver.Resolve(unresolvedText);
                if (resolvedText == unresolvedText)
                {
                    _logger.DebugFormat("The unresolved and resolved data is equal, mark source file for copy action instead of resolve action.");
                    copyAction = CopyAction.Copy;
                }
            }
            if (copyAction == CopyAction.Resolve)
            {
                var temporaryTargetFile = Path.GetTempFileName();
                _logger.Debug(sourceFile == targetFile
                    ? "Source file and target file is the same. Resolve into temporary file before overwriting source."
                    : "Resolve into temporary file before writing to target file.");
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
            else
            {
                _logger.DebugFormat("Nothing to resolve, copy the file from source to target: '{0}'->'{1}'.",sourceFile,targetFile);
                if (sourceFile != targetFile)
                {
                    _fileCopy.Copy(sourceFile, targetFile, false);
                }
                else
                {
                    _logger.DebugFormat("Nothing to copy, source file and to target file is the same. '{0}'=='{1}'", sourceFile, targetFile);
                }
            }
        }
    }
}