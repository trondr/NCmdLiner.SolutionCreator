using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Common.Logging;
using NCmdLiner.SolutionCreator.Library.Common;

namespace NCmdLiner.SolutionCreator.Library.Services
{
    [Singleton]
    public class Context : IContext
    {
        private readonly IGuidGeneator _guidGeneator;
        private readonly ILog _logger;

        public Context(IGuidGeneator guidGeneator,ILog logger)
        {
            _guidGeneator = guidGeneator;
            _logger = logger;
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
            //All context variable names on format GuidN will return a guid, one unique guid for each index N
            if (GuidRegex.IsMatch(name))
            {
                AddVariable(name, _guidGeneator.GetNewGuid());
                return Variables[name];
            }            
            _logger.WarnFormat("Context variable name '{0}' was unknown. Returning null.", name);
            return null;
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

    }
}