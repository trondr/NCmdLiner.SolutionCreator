using System.Text.RegularExpressions;

namespace NCmdLiner.SolutionCreator.Library.Services
{
    public class SolutionAttributeHelper : ISolutionAttributeHelper
    {
        public SolutionInfoAttribute GetSolutionInfoAttributeFromAttributeName(string solutionInfoAttributeName)
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