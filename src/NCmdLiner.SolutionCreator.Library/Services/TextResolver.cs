using System.Text.RegularExpressions;
using Common.Logging;

namespace NCmdLiner.SolutionCreator.Library.Services
{
    public class TextResolver : ITextResolver
    {
        private readonly IContext _context;
        private readonly ILog _logger;
        private readonly int _sLenth;
        private readonly int _eLenth;
        private readonly Regex _regEx;
        private readonly Regex _guidRegEx;

        public TextResolver(IContext context, ILog logger)
        {
            _context = context;
            _logger = logger;
            VariableStartString = "_S_";
            VariableEndString = "_S_";
            _sLenth = VariableStartString.Length;
            _eLenth = VariableEndString.Length;
            //Pattern to find in the text variable names on the format '_S_SomeVariable_S_' (where _S_ is used as start and end of variable name)
            //pattern credits: http://stackoverflow.com/questions/977251/regular-expressions-and-negating-a-whole-character-group
            var pattern = string.Format(@"({0})(?:(?!{1}).)+\1", VariableStartString, VariableEndString);
            _regEx = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);
            const string guidPattern = @"(ECD7A685-EDCC-474C-AD38-[0-9a-fA-F]{12})";
            _guidRegEx = new Regex(guidPattern, RegexOptions.IgnorePatternWhitespace);
        }

        public string Resolve(string text)
        {
            var text1 = _regEx.Replace(text, ReplaceContextVariable);
            return _guidRegEx.Replace(text1, ReplaceGuidContextVariable);
        }

        private string ReplaceContextVariable(Match match)
        {
            var cv = match.Value;
            var variableName = cv.Substring(_sLenth, cv.Length - _sLenth - _eLenth);
            var variableValue = _context.GetVariable(variableName) ?? VariableStartString + variableName + VariableEndString;
            return variableValue;
        }

        private string ReplaceGuidContextVariable(Match match)
        {            
            var variableName = match.Value;
            var variableValue = _context.GetVariable(variableName) ?? variableName;
            return variableValue;
        }

        string VariableStartString { get; set; }

        string VariableEndString { get; set; }
    }    
}