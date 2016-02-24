using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NCmdLiner.SolutionCreator.Library.Services
{
    public class SolutionAttributeSearcher : ISolutionAttributeSearcher
    {
        private readonly ISolutionAttributeHelper _solutionAttributeHelper;

        public SolutionAttributeSearcher(ISolutionAttributeHelper solutionAttributeHelper)
        {
            _solutionAttributeHelper = solutionAttributeHelper;
        }

        readonly Regex _attributeNameRegex = new Regex("(_S_.+?[^U]_S_)",RegexOptions.Compiled|RegexOptions.IgnoreCase);
        
        public IEnumerable<SolutionInfoAttribute> FindSolutionAttributesFromString(string text)
        {
            var matches = _attributeNameRegex.Matches(text);
            foreach (Match match in matches)
            {
               yield return _solutionAttributeHelper.GetSolutionInfoAttributeFromAttributeName(match.Value);
            }            
        }
    }
}