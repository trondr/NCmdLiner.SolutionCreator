using System.Collections.Generic;
using NCmdLiner.SolutionCreator.Library.Model;

namespace NCmdLiner.SolutionCreator.Library.Services
{
    public interface ISolutionTemplateProvider
    {
        IEnumerable<SolutionTemplate> GetSolutionTemplates();
    }
}