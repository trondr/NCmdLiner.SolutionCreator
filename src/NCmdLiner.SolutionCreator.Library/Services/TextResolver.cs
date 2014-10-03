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
        }

        public string Resolve(string text)
        {
            return _regEx.Replace(text, ReplaceContextVariable);
        }

        private string ReplaceContextVariable(Match match)
        {
            var cv = match.Value;
            var variableName = cv.Substring(_sLenth, cv.Length - _sLenth - _eLenth);
            var variableValue = _context.GetVariable(variableName) ?? VariableStartString + variableName + VariableEndString;
            return variableValue;
        }

        string VariableStartString { get; set; }

        string VariableEndString { get; set; }
    }    
}