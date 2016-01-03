namespace NCmdLiner.SolutionCreator.Library.Model
{
    public interface IClientServiceApplicationInfo: IConsoleApplicationInfo
    {
        string ServiceConsoleProjectName { get; set; }
        string ServiceContractsProjectName { get; set; }
        string ServiceDescription { get; set; }
        string ServiceLibraryProjectName { get; set; }
        string ServiceProjectName { get; set; }
        string ServiceScriptInstallProjectName { get; set; }        
        string ServiceSetupProjectName { get; set; }
    }
}