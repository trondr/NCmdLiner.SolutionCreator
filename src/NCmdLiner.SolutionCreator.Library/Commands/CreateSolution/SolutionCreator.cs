using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using Common.Logging;
using NCmdLiner.SolutionCreator.Library.Model;
using NCmdLiner.SolutionCreator.Library.Services;
using NCmdLiner.SolutionCreator.Library.ViewModels;
using NCmdLiner.SolutionCreator.Library.Views;

namespace NCmdLiner.SolutionCreator.Library.Commands.CreateSolution
{
    public class SolutionCreator : ISolutionCreator
    {
        private readonly MainWindow _mainWindow;
        private readonly IContext _context;
        private readonly IFolderResolver _folderResolver;
        private readonly ITemplateProvider _templateProvider;
        private readonly ILog _logger;

        public SolutionCreator(MainWindow mainWindow, IContext context, IFolderResolver folderResolver, ITemplateProvider templateProvider, ILog logger)
        {
            _mainWindow = mainWindow;
            _context = context;
            _folderResolver = folderResolver;
            _templateProvider = templateProvider;
            _logger = logger;
        }


        public int Create(string targetRootFolder)
        {
            var returnValue = 0;
            _logger.Info("Getting basic info from the user");
            var application = new System.Windows.Application();
            application.Run(_mainWindow);
            var viewModel = _mainWindow.View.ViewModel as MainViewModel;
            if (viewModel != null)
            {
                _logger.Info("Transfering basic information from the user into the resolver context dictionary.");
                _context.AddVariable("CompanyName", viewModel.CompanyName);
                _context.AddVariable("NamespaceCompany", viewModel.NamespaceCompanyName);
                _context.AddVariable("ProductName", viewModel.ProductName);
                _context.AddVariable("ShortProductName", viewModel.ShortProductName);
                _context.AddVariable("ProductDescription", viewModel.ProductDescription);
                _context.AddVariable("ConsoleProjectName", viewModel.ConsoleProjectName);
                _context.AddVariable("LibraryProjectName", viewModel.LibraryProjectName);
                _context.AddVariable("TestsProjectName", viewModel.TestsProjectName);
                _context.AddVariable("SetupProjectName", viewModel.SetupProjectName);
                _context.AddVariable("Year", DateTime.Now.Year.ToString(CultureInfo.InvariantCulture));
                _context.AddVariable("RootNamespace", viewModel.NamespaceCompanyName + "." + viewModel.ProductName);

                var targetSolutionFolder = Path.Combine(targetRootFolder, viewModel.ProductName);
                if (!Directory.Exists(targetSolutionFolder))
                {
                    var template = _templateProvider.Templates.ToList()[0];
                    _logger.InfoFormat("Creating new solution by resolving template folder '{0}' to new solution folder '{1}'.", template.Path, targetSolutionFolder);
                    _folderResolver.Resolve(template.Path, targetSolutionFolder);
                }
                else
                {
                    _logger.ErrorFormat("Cannot create new solution. The target solution folder '{0}' allready exists.", targetSolutionFolder);
                    MessageBox.Show("Cannot create new solution. Target solution folder allreay exists: " + targetSolutionFolder,"");
                    returnValue = 1;
                }
            }
            else
            {
                _logger.Fatal("Fatal error. View model returned from dialog was null");
                returnValue = 2;
            }
            return returnValue;
        }
    }
}