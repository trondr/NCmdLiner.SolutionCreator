using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCmdLiner.SolutionCreator.Library.BootStrap
{
    public interface ISolutionCreatorApplicationInfo
    {
        string Name { get; set; }
        string Version {get;set; }
    }
}
