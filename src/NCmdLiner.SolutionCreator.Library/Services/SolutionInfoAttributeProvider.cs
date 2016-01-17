using System;
using System.Collections.Generic;
using System.IO;

namespace NCmdLiner.SolutionCreator.Library.Services
{
    public class SolutionInfoAttributeProvider : ISolutionInfoAttributeProvider
    {
        private readonly ISolutionAttributeSearcher _solutionAttributeSearcher;

        public SolutionInfoAttributeProvider(ISolutionAttributeSearcher solutionAttributeSearcher)
        {
            _solutionAttributeSearcher = solutionAttributeSearcher;
        }

        public IEnumerable<SolutionInfoAttribute> GetSolutionInfoAttributesFromTemplateFolder(string solutionTemplateFolder)
        {
            if(!Directory.Exists(solutionTemplateFolder))
            {
                throw new DirectoryNotFoundException("Unable to get solution info attributes from solution template folder. Solution template folder not found: " + solutionTemplateFolder);
            }

            //Get all files in template folder
            var files = Directory.GetFiles(solutionTemplateFolder,"*.*",SearchOption.AllDirectories);

            //Search through each file for attributes on the format '_S_.+?_S_' . Example: _S_ConsoleProjectName_S_
            var uniqueSolutionInfoAttributes = new Dictionary<SolutionInfoAttribute, object>();
            foreach (var file in files)
            {
                var solutionInfoAttributes = GetUniqueSolutionInfoAttributesFromFile(file, uniqueSolutionInfoAttributes);
                foreach (var solutionInfoAttribute in solutionInfoAttributes)
                {
                    yield return solutionInfoAttribute;
                }
            }
        }

        private IEnumerable<SolutionInfoAttribute> GetUniqueSolutionInfoAttributesFromFile(string file, Dictionary<SolutionInfoAttribute, object> uniqueSolutionInfoAttributes)
        {
            using (var sr = new StreamReader(file))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    foreach (var solutionInfoAttribute in _solutionAttributeSearcher.FindSolutionAttributesFromString(line))
                    {
                        if (!uniqueSolutionInfoAttributes.ContainsKey(solutionInfoAttribute))
                        {
                            uniqueSolutionInfoAttributes.Add(solutionInfoAttribute, null);
                            yield return solutionInfoAttribute;
                        }
                    }
                }
            }
        }
    }
}