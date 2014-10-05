using System.Windows.Input;

namespace NCmdLiner.SolutionCreator.Library.ViewModels
{
    public class DesignTimeMainViewModel : ViewModelBase, IMainViewModel
    {
        public DesignTimeMainViewModel()
        {
            this.CompanyName = "My Company";
            this.CompanyNameLabelText = "Company Name:";
            this.NamespaceCompanyName = "MyCompany";
            this.NamespaceCompanyNameLabelText = "Namespace Company Name:";
            this.ProductName = "My Product";
            this.ProductNameLabelText = "Product Name:";
            this.ShortProductName = "My.Product";
            this.ShortProductNameLabelText = "Short Product Name:";
            this.ProductDescription = "My Product Description";
            this.ProductDescriptionLabelText = "Product Description:";
            this.ConsoleProjectName = "MyCompany.MyProduct";
            this.ConsoleProjectNameLabelText = "Console Project Name:";
            this.LibraryProjectName = "MyCompany.MyProduct.Library";
            this.LibraryProjectNameLabelText = "Library Project Name:";
            this.TestsProjectName = "MyCompany.MyProduct.Tests";
            this.TestsProjectNameLabelText = "Tests Project Name:";
            this.SetupProjectName = "MyCompany.MyProduct.Setup";
            this.SetupProjectNameLabelText = "Setup Project Name:";
            this.MaxLabelWidth = this.NamespaceCompanyNameLabelText.Length * 10;
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
        public ICommand OkCommand { get; set; }

    }
}