namespace NCmdLiner.SolutionCreator.Library.Model
{
    public class ConsoleApplicationInfo : IConsoleApplicationInfo
    {
        public ConsoleApplicationInfo()
        {
            CompanyName = "My Company AS";
            NamespaceCompanyName = "MyCompany";
            ProductName = "My Product";
            ShortProductName = "MyProduct";
            ProductDescription = "My Product Description";
            ConsoleProjectName = "MyCompany.MyProduct";
            LibraryProjectName = "MyCompany.MyProduct.Library";
            TestsProjectName = "MyCompany.MyProduct.Tests";
            SetupProjectName = "MyCompany.MyProduct.Setup";
            ScriptInstallProjectName = "MyCompany My Product";
            Authors = "firstname.lastname@mydomain.com";
        }

        public string CompanyName { get; set; }
        public string NamespaceCompanyName { get; set; }
        public string ProductName { get; set; }
        public string ShortProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ConsoleProjectName { get; set; }
        public string LibraryProjectName { get; set; }
        public string TestsProjectName { get; set; }
        public string SetupProjectName { get; set; }
        public string ScriptInstallProjectName { get; set; }
        public string Authors { get; set; }
    }
}
