using System;
using System.Globalization;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Windows;
using AutoMapper;
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
        private readonly ISolutionTemplateProvider _solutionTemplateProvider;
        private readonly ILog _logger;

        public SolutionCreator(MainWindow mainWindow, IContext context, IFolderResolver folderResolver, ISolutionTemplateProvider solutionTemplateProvider, ILog logger)
        {
            _mainWindow = mainWindow;
            _context = context;
            _folderResolver = folderResolver;
            _solutionTemplateProvider = solutionTemplateProvider;
            _logger = logger;
            Mapper.CreateMap<IConsoleApplicationInfo, IMainViewModel>();
        }
        
        public int Create(string targetRootFolder, IConsoleApplicationInfo consoleApplicationInfo)
        {
            var returnValue = 0;
            _logger.Info("Getting basic info from the user");
            var application = new System.Windows.Application();
            var viewModel = _mainWindow.View.ViewModel as MainViewModel;
            Mapper.Map(consoleApplicationInfo, viewModel);
            application.Run(_mainWindow);
            
            if (viewModel != null)
            {
                if (viewModel.IsFilledOut)
                {
                    _logger.Info("Transfering basic information from the user into the resolver context dictionary.");
                    _context.AddVariable("CompanyName", viewModel.CompanyName);
                    _context.AddVariable("NamespaceCompany", viewModel.NamespaceCompanyName);
                    _context.AddVariable("ProductName", viewModel.ProductName);
                    _context.AddVariable("ShortProductName", viewModel.ShortProductName);                    
                    _context.AddVariable("ProductDescription", viewModel.ProductDescription);
                    _context.AddVariable("ConsoleProjectName", viewModel.ConsoleProjectName);
                    _context.AddVariable("ConsoleProjectNameU", viewModel.ConsoleProjectName.Replace(".", "_").Replace("__", "_"));
                    _context.AddVariable("LibraryProjectName", viewModel.LibraryProjectName);
                    _context.AddVariable("LibraryProjectNameU", viewModel.LibraryProjectName.Replace(".", "_").Replace("__", "_"));
                    _context.AddVariable("TestsProjectName", viewModel.TestsProjectName);
                    _context.AddVariable("TestsProjectNameU", viewModel.TestsProjectName.Replace(".", "_").Replace("__", "_"));
                    _context.AddVariable("SetupProjectName", viewModel.SetupProjectName);
                    _context.AddVariable("SetupProjectNameU", viewModel.SetupProjectName.Replace(".", "_").Replace("__", "_"));
                    _context.AddVariable("ScriptInstallProjectName", viewModel.ScriptInstallProjectName);
                    _context.AddVariable("ScriptInstallProjectNameU", viewModel.ScriptInstallProjectName.Replace(" ", "_").Replace(".", "_").Replace("__", "_"));
                    _context.AddVariable("Year", DateTime.Now.Year.ToString(CultureInfo.InvariantCulture));
                    _context.AddVariable("RootNamespace", viewModel.NamespaceCompanyName + "." + viewModel.ProductName);
                    _context.AddVariable("Authors", viewModel.Authors);
                    var targetSolutionFolder = Path.Combine(targetRootFolder, viewModel.ShortProductName);
                    if (!Directory.Exists(targetSolutionFolder))
                    {
                        var templates = _solutionTemplateProvider.SolutionTemplates.ToList();
                        _logger.Debug("Found number of templates: " + templates.Count);
                        if (templates.Count > 0)
                        {
                            var template = templates[0];
                            _logger.InfoFormat("Creating new solution by resolving template folder '{0}' to new solution folder '{1}'.", template.Path, targetSolutionFolder);
                            _folderResolver.Resolve(template.Path, targetSolutionFolder);
                        }
                        else
                        {
                            throw new Exception("No availble templates to resolve");
                        }
                    }
                    else
                    {
                        _logger.ErrorFormat("Cannot create new solution. The target solution folder '{0}' allready exists.", targetSolutionFolder);
                        MessageBox.Show("Cannot create new solution. Target solution folder allreay exists: " + targetSolutionFolder, "");
                        returnValue = 1;
                    }
                }
                else
                {
                    _logger.Error("Cannot create new solution. All fields were not filled out.");
                    MessageBox.Show("Cannot create new solution. All fields were not filled out.");
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