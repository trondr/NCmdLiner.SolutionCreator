using NCmdLiner.SolutionCreator.Library.Common;
using NCmdLiner.SolutionCreator.Library.Model;
using NCmdLiner.SolutionCreator.Library.Views;

namespace NCmdLiner.SolutionCreator.Library.Commands.CreateSolution
{
    public class SolutionCreatorCommandProvider : CommandProvider, ISolutionCreatorCommandProvider
    {
        private readonly ICreateSolutionApplication _createSolutionApplication;

        public SolutionCreatorCommandProvider(ICreateSolutionApplication createSolutionApplication)
        {
            _createSolutionApplication = createSolutionApplication;
        }

        public int CreateSolution(string targetRootFolder, IConsoleApplicationInfo consoleApplicationInfo)
        {
            var returnValue = 0;
            _createSolutionApplication.InitializeAndRun(targetRootFolder);
            return _createSolutionApplication.ExitCode;

            //var solutionInfoWindow = _solutionInfoWindowFactory.GetSolutionInfoWindow();
            //var view = solutionInfoWindow.View;
            //var viewModel = view.ViewModel;
            //var solutionInfoAttributes = _solutionInfoAttributeProvider.GetSolutionInfoAttributesFromTemplateFolder(selectedTemplate.Path);
            //foreach (var solutionInfoAttribute in solutionInfoAttributes)
            //{
            //    viewModel.SolutionInfoAttributes.Add(_typeMapper.Map<SolutionInfoAttributeViewModel>(solutionInfoAttribute));
            //}
            //var application = new Application();
            //var returnCode = application.Run(solutionInfoWindow);
            //_logger.Info("Dialog result: " + returnCode);

            //application = new System.Windows.Application();
            //viewModel = _mainWindow.View.ViewModel as MainViewModel;
            //Mapper.Map(consoleApplicationInfo, viewModel);
            //application.Run(_mainWindow);

            //if (viewModel != null)
            //{
            //    if (viewModel.IsFilledOut)
            //    {
            //        _logger.Info("Transfering basic information from the user into the resolver context dictionary.");
            //        _resolveContext.AddVariable("CompanyName", viewModel.CompanyName);
            //        _resolveContext.AddVariable("NamespaceCompany", viewModel.NamespaceCompanyName);
            //        _resolveContext.AddVariable("ProductName", viewModel.ProductName);
            //        _resolveContext.AddVariable("ShortProductName", viewModel.ShortProductName);
            //        _resolveContext.AddVariable("ProductDescription", viewModel.ProductDescription);
            //        _resolveContext.AddVariable("ConsoleProjectName", viewModel.ConsoleProjectName);
            //        _resolveContext.AddVariable("ConsoleProjectNameU", viewModel.ConsoleProjectName.Replace(".", "_").Replace("__", "_"));
            //        _resolveContext.AddVariable("LibraryProjectName", viewModel.LibraryProjectName);
            //        _resolveContext.AddVariable("LibraryProjectNameU", viewModel.LibraryProjectName.Replace(".", "_").Replace("__", "_"));
            //        _resolveContext.AddVariable("TestsProjectName", viewModel.TestsProjectName);
            //        _resolveContext.AddVariable("TestsProjectNameU", viewModel.TestsProjectName.Replace(".", "_").Replace("__", "_"));
            //        _resolveContext.AddVariable("SetupProjectName", viewModel.SetupProjectName);
            //        _resolveContext.AddVariable("SetupProjectNameU", viewModel.SetupProjectName.Replace(".", "_").Replace("__", "_"));
            //        _resolveContext.AddVariable("ScriptInstallProjectName", viewModel.ScriptInstallProjectName);
            //        _resolveContext.AddVariable("ScriptInstallProjectNameU", viewModel.ScriptInstallProjectName.Replace(" ", "_").Replace(".", "_").Replace("__", "_"));
            //        _resolveContext.AddVariable("Year", DateTime.Now.Year.ToString(CultureInfo.InvariantCulture));
            //        _resolveContext.AddVariable("RootNamespace", viewModel.NamespaceCompanyName + "." + viewModel.ProductName);
            //        _resolveContext.AddVariable("Authors", viewModel.Authors);
            //        var targetSolutionFolder = Path.Combine(targetRootFolder, viewModel.ShortProductName);
            //        if (!Directory.Exists(targetSolutionFolder))
            //        {
            //            var templates = _solutionTemplateProvider.SolutionTemplates.ToList();
            //            _logger.Debug("Found number of templates: " + templates.Count);
            //            if (templates.Count > 0)
            //            {
            //                var template = templates[0];
            //                _logger.InfoFormat("Creating new solution by resolving template folder '{0}' to new solution folder '{1}'.", template.Path, targetSolutionFolder);
            //                _folderResolver.Resolve(template.Path, targetSolutionFolder);
            //            }
            //            else
            //            {
            //                throw new Exception("No availble templates to resolve");
            //            }
            //        }
            //        else
            //        {
            //            _logger.ErrorFormat("Cannot create new solution. The target solution folder '{0}' allready exists.", targetSolutionFolder);
            //            MessageBox.Show("Cannot create new solution. Target solution folder allreay exists: " + targetSolutionFolder, "");
            //            returnValue = 1;
            //        }
            //    }
            //    else
            //    {
            //        _logger.Error("Cannot create new solution. All fields were not filled out.");
            //        MessageBox.Show("Cannot create new solution. All fields were not filled out.");
            //        returnValue = 1;
            //    }
            //}
            //else
            //{
            //    _logger.Fatal("Fatal error. View model returned from dialog was null");
            //    returnValue = 2;
            //}
            return returnValue;
        }

        
    }
}