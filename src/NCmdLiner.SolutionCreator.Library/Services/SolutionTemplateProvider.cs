using System.Collections.Generic;
using System.IO;
using Common.Logging;
using NCmdLiner.SolutionCreator.Library.BootStrap;
using NCmdLiner.SolutionCreator.Library.Model;

namespace NCmdLiner.SolutionCreator.Library.Services
{
    public class SolutionTemplateProvider : ISolutionTemplateProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ILog _logger;

        public SolutionTemplateProvider(IConfiguration configuration, ILog logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IEnumerable<SolutionTemplate> SolutionTemplates
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
                            _logger.Debug("Template folder exists: " + templateDirectoryInfo.FullName);
                            foreach (var subDirectory in templateDirectoryInfo.GetDirectories())
                            {
                                yield return new SolutionTemplate() {Name = subDirectory.Name, Path = subDirectory.FullName};
                            }                            
                        }
                        else
                        {
                            _logger.Debug("Template folder does NOT exist: " + templateDirectoryInfo.FullName);
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
        private IEnumerable<SolutionTemplate> _templates;
    }
}