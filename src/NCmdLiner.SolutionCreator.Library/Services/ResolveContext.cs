using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Common.Logging;
using NCmdLiner.SolutionCreator.Library.Common;

namespace NCmdLiner.SolutionCreator.Library.Services
{
    [Singleton]
    public class ResolveContext : IResolveContext
    {
        private readonly IGuidGeneator _guidGeneator;
        private readonly ILog _logger;

        public ResolveContext(IGuidGeneator guidGeneator,ILog logger)
        {
            _guidGeneator = guidGeneator;
            _logger = logger;
            var pattern = string.Format(@"([\.\-;,\s]+)");
            _regEx = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);
        }

        public void AddVariable(string name, string value)
        {
            if (Variables.ContainsKey(name))
            {
                Variables[name] = value;
            }
            else
            {
                Variables.Add(name,value);
            }
        }

        public string GetVariable(string name)
        {
            if (Variables.ContainsKey(name))
            {
                return Variables[name];
            }
            //All variables ending with U_S_ will be looked up by the corresponding 
            //variable without U_S_ ending. Example: _S_SomeValueU_S_ will be looked up 
            //using _S_SomeValue_S_ and the value will have all white spaces and special
            //characters replaced by underscore.
            var underscoredValue = GetUnderscoredValue(name);
            if(underscoredValue != null)
            {
                return  underscoredValue;
            }
            
            //All context variable names on format GuidN will return a guid, one unique guid for each index N
            if (GuidRegex.IsMatch(name) || SpecialGuidRegex.IsMatch(name))
            {
                AddVariable(name, _guidGeneator.GetNewGuid());
                return Variables[name];
            }            
            _logger.WarnFormat("Context variable name '{0}' was unknown. Returning null.", name);
            return null;
        }

        private string GetUnderscoredValue(string name)
        {
            if(name.EndsWith("U_S_"))
            {
                var nameWithOutU = name.Replace("U_S_","_S_");
                if (Variables.ContainsKey(nameWithOutU))
                {
                    return _regEx.Replace(Variables[nameWithOutU], "_");
                }
            }
            return  null;
        }
        
        private IDictionary<string, string> Variables
        {
            get { return _variables ?? (_variables = new Dictionary<string, string>()); }
        }
        private Dictionary<string, string> _variables;

        private Regex GuidRegex
        {
            get { return _guidRegex ?? (_guidRegex = new Regex(@"^Guid\d+$")); }
        }
        private Regex _guidRegex;

        private Regex SpecialGuidRegex
        {
            get 
            {
                if (_specialGuidRegex != null) return _specialGuidRegex;
                const string guidPattern = @"ECD7A685-EDCC-474C-AD38-[0-9a-fA-F]{12}";
                _specialGuidRegex = new Regex(guidPattern, RegexOptions.IgnorePatternWhitespace);
                return _specialGuidRegex;
            }
        }
        private Regex _specialGuidRegex;
        private Regex _regEx;
    }
}