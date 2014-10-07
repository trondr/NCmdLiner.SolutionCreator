using System.Collections.Generic;

namespace NCmdLiner.SolutionCreator.Library.BootStrap
{
    public interface IConfiguration
    {
        string LogDirectoryPath { get; set; }
        string LogFileName { get; set; }
        IEnumerable<string> TemplatesFolders { get; set; } 
    }
}
