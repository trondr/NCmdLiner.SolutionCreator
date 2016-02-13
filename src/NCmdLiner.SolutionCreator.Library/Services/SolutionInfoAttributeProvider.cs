using System;
using System.Collections.Generic;
using System.IO;
using NCmdLiner.SolutionCreator.Library.Common.IO;

namespace NCmdLiner.SolutionCreator.Library.Services
{
    public class SolutionInfoAttributeProvider : ISolutionInfoAttributeProvider
    {
        private readonly ISolutionAttributeSearcher _solutionAttributeSearcher;
        private readonly ISolutionInfoAttributeFilesProvider _solutionInfoAttributeFilesProvider;
        private readonly IIniFileOperation _iniFileOperation;
        private readonly ISolutionAttributeHelper _solutionAttributeHelper;

        public SolutionInfoAttributeProvider(ISolutionAttributeSearcher solutionAttributeSearcher, ISolutionInfoAttributeFilesProvider solutionInfoAttributeFilesProvider, IIniFileOperation iniFileOperation, ISolutionAttributeHelper solutionAttributeHelper)
        {
            _solutionAttributeSearcher = solutionAttributeSearcher;
            _solutionInfoAttributeFilesProvider = solutionInfoAttributeFilesProvider;
            _iniFileOperation = iniFileOperation;
            _solutionAttributeHelper = solutionAttributeHelper;
        }

        public IEnumerable<SolutionInfoAttribute> GetSolutionInfoAttributesFromTemplateFolder(string solutionTemplateFolder)
        {
            if(!Directory.Exists(solutionTemplateFolder))
            {
                throw new DirectoryNotFoundException("Unable to get solution info attributes from solution template folder. Solution template folder not found: " + solutionTemplateFolder);
            }
            var uniqueSolutionInfoAttributes = new Dictionary<SolutionInfoAttribute, object>();
            //Get solution attributes with default values from SolutionAttributes.ini
            var solutionAttributesIni = Path.Combine(solutionTemplateFolder,"SolutionAttributes.ini");
            if(File.Exists(solutionAttributesIni))
            {
                var solutionInfoAttributes = GetUniqueSolutionInfoAttributesFromAttributesIni(solutionAttributesIni, uniqueSolutionInfoAttributes);
                foreach (var solutionInfoAttribute in solutionInfoAttributes)
                {
                    yield return solutionInfoAttribute;
                }
            }


            //Get all files in template folder, search through each file for attributes on the format '_S_.+?_S_' . Example: _S_ConsoleProjectName_S_
            foreach (var file in _solutionInfoAttributeFilesProvider.GetFiles(solutionTemplateFolder))
            {
                var solutionInfoAttributes = GetUniqueSolutionInfoAttributesFromFile(file, uniqueSolutionInfoAttributes);
                foreach (var solutionInfoAttribute in solutionInfoAttributes)
                {
                    if (!uniqueSolutionInfoAttributes.ContainsKey(solutionInfoAttribute))
                    {
                        uniqueSolutionInfoAttributes.Add(solutionInfoAttribute, null);
                        yield return solutionInfoAttribute;
                    }
                }
            }
        }

        private IEnumerable<SolutionInfoAttribute> GetUniqueSolutionInfoAttributesFromAttributesIni(string solutionAttributesIni, Dictionary<SolutionInfoAttribute, object> uniqueSolutionInfoAttributes)
        {
            foreach (var solutionAttributeKey in _iniFileOperation.GetKeys(solutionAttributesIni,"SolutionAttributes"))
            {
                var solutionAttribute = _solutionAttributeHelper.GetSolutionInfoAttributeFromAttributeName(solutionAttributeKey);
                solutionAttribute.Value = _iniFileOperation.Read(solutionAttributesIni,"SolutionAttributes",solutionAttributeKey);
                yield return  solutionAttribute;
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