using NCmdLiner.SolutionCreator.Library.Common;

namespace NCmdLiner.SolutionCreator.Library.BootStrap
{
    public class SolutionCreatorApplicationInfo : ISolutionCreatorApplicationInfo
    {
        public SolutionCreatorApplicationInfo()
        {
            Name = "NCmdLiner SolutionCreator";
            Version = ApplicationInfoHelper.ApplicationVersion;
        }

        public string Name { get; set; }
        public string Version { get; set; }
    }
}