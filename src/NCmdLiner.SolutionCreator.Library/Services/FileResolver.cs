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
        private readonly ILog _logger;

        public FileResolver(IFileEncoding fileEncoding, ITextResolver textResolver, ILog logger)
        {
            _fileEncoding = fileEncoding;
            _textResolver = textResolver;
            _logger = logger;
        }

        public void Resolve(string sourceFile, string targetFile)
        {
            ToDo.Implement(ToDoPriority.Critical, "trondr","Implement resolve of file");
            var fileEncoding = _fileEncoding.GetEncoding(sourceFile);
            var resolvedText = new StringBuilder();
            using (var sr = new StreamReader(sourceFile, fileEncoding))
            {                
                resolvedText.Append(_textResolver.Resolve(sr.ReadToEnd()));
            }            
            var temporaryTargetFile = Path.GetTempFileName();
            _logger.Debug(sourceFile == targetFile ? "Source file and target file is the same. Resolve into temporary file before overwriting source." : "Resolve into temporary file before writing to target file.");
            using (var sw = new StreamWriter(temporaryTargetFile, false, fileEncoding))
            {
                sw.Write(resolvedText);
            }
            if (File.Exists(targetFile))
            {
                File.Delete(targetFile);
            }
            File.Move(temporaryTargetFile,targetFile);
        }
    }
}