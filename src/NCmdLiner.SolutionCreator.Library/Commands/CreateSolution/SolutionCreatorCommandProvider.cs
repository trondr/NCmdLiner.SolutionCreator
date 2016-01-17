using System;
using System.Globalization;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Windows;
using AutoMapper;
using Common.Logging;
using NCmdLiner.SolutionCreator.Library.BootStrap;
using NCmdLiner.SolutionCreator.Library.Common;
using NCmdLiner.SolutionCreator.Library.Common.UI;
using NCmdLiner.SolutionCreator.Library.Model;
using NCmdLiner.SolutionCreator.Library.Services;
using NCmdLiner.SolutionCreator.Library.ViewModels;
using NCmdLiner.SolutionCreator.Library.Views;

namespace NCmdLiner.SolutionCreator.Library.Commands.CreateSolution
{
    public class SolutionCreatorCommandProvider : CommandProvider, ISolutionCreatorCommandProvider
    {
        private readonly MainWindow _mainWindow;
        private readonly IResolveContext _resolveContext;
        private readonly IFolderResolver _folderResolver;
        private readonly ISolutionTemplateProvider _solutionTemplateProvider;
        private readonly ISelectSolutionTemplateWindowFactory _selectSolutionTemplateWindowFactory;
        private readonly ISolutionInfoWindowFactory _solutionInfoWindowFactory;
        private readonly ISolutionInfoAttributeProvider _solutionInfoAttributeProvider;
        private readonly ITypeMapper _typeMapper;
        private readonly ILog _logger;

        public SolutionCreatorCommandProvider(MainWindow mainWindow, IResolveContext resolveContext, IFolderResolver folderResolver, ISolutionTemplateProvider solutionTemplateProvider, ISelectSolutionTemplateWindowFactory selectSolutionTemplateWindowFactory, ISolutionInfoWindowFactory solutionInfoWindowFactory,ISolutionInfoAttributeProvider solutionInfoAttributeProvider,ITypeMapper typeMapper, ILog logger)
        {
            _mainWindow = mainWindow;
            _resolveContext = resolveContext;
            _folderResolver = folderResolver;
            _solutionTemplateProvider = solutionTemplateProvider;
            _selectSolutionTemplateWindowFactory = selectSolutionTemplateWindowFactory;
            _solutionInfoWindowFactory = solutionInfoWindowFactory;
            _solutionInfoAttributeProvider = solutionInfoAttributeProvider;
            _typeMapper = typeMapper;
            _logger = logger;
            //Mapper.CreateMap<IConsoleApplicationInfo, IMainViewModel>();
        }

        public int CreateSolution(string targetRootFolder, IConsoleApplicationInfo consoleApplicationInfo)
        {
            var returnValue = 0;
            _logger.Info("Display select solution dialog to the user...");
            var selectedTemplate = DisplaySelectSolutionDialogToTheUser();
            if (selectedTemplate == null)
            {
                _logger.Warn("User canceled the operation of selecting solution template.");
                return 0;
            }
            _logger.InfoFormat("User selected template: {0} ({1})", selectedTemplate.Name, selectedTemplate.Path);

            _logger.Info("Getting basic info from the user");
            var solutionInfoWindow = _solutionInfoWindowFactory.GetSolutionInfoWindow();
            var view = solutionInfoWindow.View;
            var viewModel = view.ViewModel;
            var solutionInfoAttributes = _solutionInfoAttributeProvider.GetSolutionInfoAttributesFromTemplateFolder(selectedTemplate.Path);
            foreach (var solutionInfoAttribute in solutionInfoAttributes)
            {
                viewModel.SolutionInfoAttributes.Add(_typeMapper.Map<SolutionInfoAttributeViewModel>(solutionInfoAttribute));
            }
            var application = new Application();
            application.Run(solutionInfoWindow);
            _logger.Info("Dialog result: " + solutionInfoWindow.DialogResult);
            
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

        private SolutionTemplate DisplaySelectSolutionDialogToTheUser()
        {
            SelectSolutionTemplateWindow window = null;
            try
            {
                window = _selectSolutionTemplateWindowFactory.GetSelectSolutionTemplateWindow();
                var view = window.View;
                var viewModel = view.ViewModel;
                foreach (var solutionTemplate in _solutionTemplateProvider.GetSolutionTemplates())
                {
                    viewModel.SolutionTemplates.Add(_typeMapper.Map<SolutionTemplateViewModel>(solutionTemplate));
                }
                var application = new Application();
                application.Run(window);
                return GetSelectedSolutionTemplate(window);
            }
            finally
            {
                _selectSolutionTemplateWindowFactory.Release(window);
            }
        }

        private SolutionTemplate GetSelectedSolutionTemplate(SelectSolutionTemplateWindow window)
        {
            if (window.DialogResult != null && window.DialogResult.Value == DialogResult.Ok)
            {
                return _typeMapper.Map<SolutionTemplate>(window.View.ViewModel.SelectedSolutionTemplate);
            }
            return null;
        }
    }
}