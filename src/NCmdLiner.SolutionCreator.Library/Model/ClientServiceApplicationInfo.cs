namespace NCmdLiner.SolutionCreator.Library.Model
{
    public class ClientServiceApplicationInfo : ConsoleApplicationInfo, IClientServiceApplicationInfo
    {
        public ClientServiceApplicationInfo()
        {
            ServiceConsoleProjectName = base.ConsoleProjectName + ".Service.Console";
            ServiceContractsProjectName = base.ConsoleProjectName + ".Service.Contracts";
            ServiceDescription = "My Service Description";
            ServiceLibraryProjectName = base.ConsoleProjectName + ".Service.Library";
            ServiceProjectName = base.ConsoleProjectName + ".Service";
            ServiceScriptInstallProjectName = base.ScriptInstallProjectName + " Service";
            ServiceSetupProjectName = base.ConsoleProjectName + ".Service.Setup";
        }

        public string ServiceConsoleProjectName { get; set; }
        public string ServiceContractsProjectName { get; set; }
        public string ServiceDescription { get; set; }
        public string ServiceLibraryProjectName { get; set; }
        public string ServiceProjectName { get; set; }
        public string ServiceScriptInstallProjectName { get; set; }
        public string ServiceSetupProjectName { get; set; }
    }
}