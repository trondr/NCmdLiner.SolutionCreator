using System.Collections.Generic;
using System.Threading.Tasks;

namespace NCmdLiner.SolutionCreator.Library.Services
{
    public interface ISolutionInfoAttributeProvider
    {
        IEnumerable<SolutionInfoAttribute> GetSolutionInfoAttributesFromTemplateFolder(string solutionTemplateFolder);
    }
}