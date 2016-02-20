using System.Collections.Generic;
using System.IO;

namespace NCmdLiner.SolutionCreator.Library.Services
{
    public class SolutionInfoAttributeFilesProvider : ISolutionInfoAttributeFilesProvider
    {
        private readonly ICheckFileExtentions _checkFileExtentions;

        public SolutionInfoAttributeFilesProvider(ICheckFileExtentions checkFileExtentions)
        {
            _checkFileExtentions = checkFileExtentions;
        }

        public IEnumerable<string> GetFiles(string solutionTemplateFolder)
        {
            var files = Directory.GetFiles(solutionTemplateFolder, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var fileExtension = Path.GetExtension(file);
                if (_checkFileExtentions.IsBinaryFileExtension(fileExtension))
                    continue;

                yield return file;
            }
        }        
    }
}