using System.Collections.Generic;
using System.IO;
using NCmdLiner.SolutionCreator.Library.BootStrap;
using NCmdLiner.SolutionCreator.Library.Model;

namespace NCmdLiner.SolutionCreator.Library.Commands.CreateSolution
{
    public class TemplateProvider : ITemplateProvider
    {
        private readonly IConfiguration _configuration;

        public TemplateProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<Template> Templates
        {
            get
            {
                if (_templates == null)
                {
                    foreach (var templateFolder in _configuration.TemplatesFolders)
                    {
                        var templateDirectoryInfo = new DirectoryInfo(templateFolder);
                        if (templateDirectoryInfo.Exists)
                        {
                            foreach (var subDirectory in templateDirectoryInfo.GetDirectories())
                            {
                                yield return new Template() {Name = subDirectory.Name, Path = subDirectory.FullName};
                            }                            
                        }
                    }
                }
                else
                {
                    foreach (var template in _templates)
                    {
                        yield return template;
                    }
                }                
            }
            set { _templates = value; }
        }
        private IEnumerable<Template> _templates;
    }
}