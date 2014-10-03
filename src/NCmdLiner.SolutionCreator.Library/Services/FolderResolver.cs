using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media.Media3D;
using Common.Logging;
using NCmdLiner.SolutionCreator.Library.Common;

namespace NCmdLiner.SolutionCreator.Library.Services
{
    public class FolderResolver : IFolderResolver
    {
        private readonly ITextResolver _textResolver;
        private readonly IFileResolver _fileResolver;
        private readonly ILog _logger;

        public FolderResolver(ITextResolver textResolver, IFileResolver fileResolver,ILog logger)
        {
            _textResolver = textResolver;
            _fileResolver = fileResolver;
            _logger = logger;
        }

        public void Resolve(string sourceFolder, string targetFolder)
        {
            ToDo.CodeReview(ToDoPriority.Critical,"trondr","Implement resolve folder");
            if(!Directory.Exists(sourceFolder)) throw new DirectoryNotFoundException("Source folder not found: " + sourceFolder);
            
            var sourceDirectory = new DirectoryInfo(sourceFolder);

            var sourceDirectories = sourceDirectory.GetDirectories("*", SearchOption.AllDirectories).ToList();
            var sourceFiles = sourceDirectory.GetFiles("*", SearchOption.AllDirectories).ToList();
            foreach (var sourceSubDirectory in sourceDirectories)
            {
                var targetSubDirectory = new DirectoryInfo(_textResolver.Resolve(sourceSubDirectory.FullName.Replace(sourceFolder, targetFolder)));
                Directory.CreateDirectory(targetSubDirectory.FullName);
            }

            foreach (var sourceFile in sourceFiles)
            {
                var targetFile = new FileInfo(_textResolver.Resolve(sourceFile.FullName.Replace(sourceFolder, targetFolder)));
                _fileResolver.Resolve(sourceFile.FullName, targetFile.FullName);                
            }
        }
    }
}