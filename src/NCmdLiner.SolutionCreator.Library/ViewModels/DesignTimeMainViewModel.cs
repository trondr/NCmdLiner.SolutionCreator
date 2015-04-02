using System.Windows.Input;

namespace NCmdLiner.SolutionCreator.Library.ViewModels
{
    public class DesignTimeMainViewModel : ViewModelBase, IMainViewModel
    {
        public DesignTimeMainViewModel()
        {
            CompanyName = "My Company";
            CompanyNameLabelText = "Company Name:";
            NamespaceCompanyName = "MyCompany";
            NamespaceCompanyNameLabelText = "Namespace Company Name:";
            ProductName = "My Product";
            ProductNameLabelText = "Product Name:";
            ShortProductName = "My.Product";
            ShortProductNameLabelText = "Short Product Name:";
            ProductDescription = "My Product Description";
            ProductDescriptionLabelText = "Product Description:";
            ConsoleProjectName = "MyCompany.MyProduct";
            ConsoleProjectNameLabelText = "Console Project Name:";
            LibraryProjectName = "MyCompany.MyProduct.Library";
            LibraryProjectNameLabelText = "Library Project Name:";
            TestsProjectName = "MyCompany.MyProduct.Tests";
            TestsProjectNameLabelText = "Tests Project Name:";
            SetupProjectName = "MyCompany.MyProduct.Setup";
            SetupProjectNameLabelText = "Setup Project Name:";
            AuthorsLableText = "Authors:";
            Authors = "dev1@some.domain.com, dev2@some.domain.com";
            MaxLabelWidth = NamespaceCompanyNameLabelText.Length * 10;
        }

        public string NamespaceCompanyNameLabelText { get; set; }
        public string ProductName { get; set; }
        public string ProductNameLabelText { get; set; }
        public string ShortProductName { get; set; }
        public string ShortProductNameLabelText { get; set; }
        public int MaxLabelWidth { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNameLabelText { get; set; }
        public string NamespaceCompanyName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductDescriptionLabelText { get; set; }
        public string ConsoleProjectName { get; set; }
        public string ConsoleProjectNameLabelText { get; set; }
        public string LibraryProjectName { get; set; }
        public string LibraryProjectNameLabelText { get; set; }
        public string TestsProjectName { get; set; }
        public string TestsProjectNameLabelText { get; set; }
        public string SetupProjectName { get; set; }
        public string SetupProjectNameLabelText { get; set; }
        public string Authors { get; set; }
        public string AuthorsLableText { get; set; }
        public ICommand OkCommand { get; set; }

    }
}