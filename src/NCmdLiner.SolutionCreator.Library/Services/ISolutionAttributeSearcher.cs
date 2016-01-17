using System.Collections.Generic;

namespace NCmdLiner.SolutionCreator.Library.Services
{
    public interface ISolutionAttributeSearcher
    {
        IEnumerable<SolutionInfoAttribute> FindSolutionAttributesFromString(string text);
    }
}