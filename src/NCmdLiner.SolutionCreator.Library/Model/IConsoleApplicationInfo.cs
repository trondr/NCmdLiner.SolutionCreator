namespace NCmdLiner.SolutionCreator.Library.Model
{
    public interface IConsoleApplicationInfo
    {
        string CompanyName { get; set; }
        string NamespaceCompanyName { get; set; }
        string ProductName { get; set; }
        string ShortProductName { get; set; }
        string ProductDescription { get; set; }
        string ConsoleProjectName { get; set; }
        string LibraryProjectName { get; set; }
        string TestsProjectName { get; set; }
        string SetupProjectName { get; set; }
        string ScriptInstallProjectName { get; set; }
        string Authors { get; set; }
    }
}