using System.Collections.Generic;
using NCmdLiner.SolutionCreator.Library.Model;

namespace NCmdLiner.SolutionCreator.Library.Commands.CreateSolution
{
    public interface ITemplateProvider
    {
        IEnumerable<Template> Templates { get; set; }
    }
}