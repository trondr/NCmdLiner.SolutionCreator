using System.Collections.Generic;
using System.IO;
using Common.Logging;
using NCmdLiner.SolutionCreator.Library.BootStrap;
using NCmdLiner.SolutionCreator.Library.Model;

namespace NCmdLiner.SolutionCreator.Library.Services
{
    public class TemplateProvider : ITemplateProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ILog _logger;

        public TemplateProvider(IConfiguration configuration, ILog logger)
        {
            _configuration = configuration;
            _logger = logger;
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
                            _logger.Debug("Template folder exists: " + templateDirectoryInfo.FullName);
                            foreach (var subDirectory in templateDirectoryInfo.GetDirectories())
                            {
                                yield return new Template() {Name = subDirectory.Name, Path = subDirectory.FullName};
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
        private IEnumerable<Template> _templates;
    }
}