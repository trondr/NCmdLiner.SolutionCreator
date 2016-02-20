using System.Collections.Generic;

namespace NCmdLiner.SolutionCreator.Library.Services
{
    public interface ISolutionInfoAttributeFilesProvider
    {
        IEnumerable<string> GetFiles(string solutionTemplateFolder);
    }
}