using System.Windows.Input;

namespace NCmdLiner.SolutionCreator.Library.ViewModels
{
    public interface IMainViewModel
    {
        int MaxLabelWidth { get; set; }
        string CompanyName { get; set; }
        string CompanyNameLabelText { get; set; }
        string NamespaceCompanyName { get; set; }
        string NamespaceCompanyNameLabelText { get; set; }
        string ProductName { get; set; }
        string ProductNameLabelText { get; set; }
        string ShortProductName { get; set; }
        string ShortProductNameLabelText { get; set; }
        string ProductDescription { get; set; }
        string ProductDescriptionLabelText { get; set; }
        string ConsoleProjectName { get; set; }
        string ConsoleProjectNameLabelText { get; set; }
        string LibraryProjectName { get; set; }
        string LibraryProjectNameLabelText { get; set; }
        string TestsProjectName { get; set; }
        string TestsProjectNameLabelText { get; set; }
        string SetupProjectName { get; set; }
        string SetupProjectNameLabelText { get; set; }
        string Authors { get; set; }
        string AuthorsLableText { get; set; }
        string ScriptInstallProjectName { get; set; }
        string ScriptInstallProjectNameLabelText { get; set; }
        ICommand OkCommand { get; set; }
    }
}
