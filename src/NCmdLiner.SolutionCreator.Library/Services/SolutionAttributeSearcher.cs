using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NCmdLiner.SolutionCreator.Library.Services
{
    public class SolutionAttributeSearcher : ISolutionAttributeSearcher
    {
        readonly Regex _attributeNameRegex = new Regex("(_S_.+?_S_)",RegexOptions.Compiled|RegexOptions.IgnoreCase);
        
        public IEnumerable<SolutionInfoAttribute> FindSolutionAttributesFromString(string text)
        {
            var matches = _attributeNameRegex.Matches(text);
            foreach (Match match in matches)
            {
               yield return GetSolutionInfoAttributeFromAttributeName(match.Value);
            }            
        }

        private SolutionInfoAttribute GetSolutionInfoAttributeFromAttributeName(string solutionInfoAttributeName)
        {
            return new SolutionInfoAttribute()
            {
                Name = solutionInfoAttributeName,
                DisplayName = GetDisplayNameFromAttributeName(solutionInfoAttributeName)
            };
        }

        private string GetDisplayNameFromAttributeName(string solutionInfoAttributeName)
        {
            var attributeNameBase = solutionInfoAttributeName.Replace("_S_","");
            return Regex.Replace(attributeNameBase, "[a-z][A-Z]", m => m.Value[0] + " " + m.Value[1]);
        }
    }
}