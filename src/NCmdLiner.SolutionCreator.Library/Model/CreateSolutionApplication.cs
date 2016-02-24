using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Common.Logging;
using NCmdLiner.SolutionCreator.Library.BootStrap;
using NCmdLiner.SolutionCreator.Library.Common.UI;
using NCmdLiner.SolutionCreator.Library.Services;
using NCmdLiner.SolutionCreator.Library.ViewModels;
using NCmdLiner.SolutionCreator.Library.Views;

namespace NCmdLiner.SolutionCreator.Library.Model
{
    public class CreateSolutionApplication : Application, ICreateSolutionApplication
    {
        private readonly ISolutionInfoWindowFactory _solutionInfoWindowFactory;
        private readonly ISelectSolutionTemplateWindowFactory _selectSolutionTemplateWindowFactory;
        private readonly ISolutionTemplateProvider _solutionTemplateProvider;
        private readonly ISolutionInfoAttributeProvider _solutionInfoAttributeProvider;
        private readonly ITypeMapper _typeMapper;
        private readonly IResolveContext _resolveContext;
        private readonly IFolderResolver _folderResolver;
        private readonly ILog _logger;
        private readonly ISolutionCreatorApplicationInfo _solutionCreatorApplicationInfo;
        private string _targetRootFolder;

        public CreateSolutionApplication(ISolutionInfoWindowFactory solutionInfoWindowFactory, ISelectSolutionTemplateWindowFactory selectSolutionTemplateWindowFactory, ISolutionTemplateProvider solutionTemplateProvider, ISolutionInfoAttributeProvider solutionInfoAttributeProvider, ITypeMapper typeMapper, IResolveContext resolveContext,IFolderResolver folderResolver, ILog logger, ISolutionCreatorApplicationInfo solutionCreatorApplicationInfo)
        {
            _solutionInfoWindowFactory = solutionInfoWindowFactory;
            _selectSolutionTemplateWindowFactory = selectSolutionTemplateWindowFactory;
            _solutionTemplateProvider = solutionTemplateProvider;
            _solutionInfoAttributeProvider = solutionInfoAttributeProvider;
            _typeMapper = typeMapper;
            _resolveContext = resolveContext;
            _folderResolver = folderResolver;
            _logger = logger;
            _solutionCreatorApplicationInfo = solutionCreatorApplicationInfo;
            Startup+=OnStartup;            
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }

        private void OnStartup(object sender, StartupEventArgs startupEventArgs)
        {
            _logger.Info("Display select solution dialog to the user...");
            var selectedSolutionTemplate = SelectSolutionTemplate();
            if (selectedSolutionTemplate == null)
            {
                _logger.Warn("User canceled the operation of selecting solution template.");
                ExitCode = 1;
                return;
            }
            _logger.InfoFormat("User selected template: {0} ({1})", selectedSolutionTemplate.Name, selectedSolutionTemplate.Path);
            var createAction = FillInSolutionInfoAttributes(selectedSolutionTemplate);            
            if(createAction == CreateAction.DoCreateSolution)
            {
                var targetSolutionFolder = Path.Combine(_targetRootFolder, GetSolutionName());
                if(Directory.Exists(targetSolutionFolder))
                {
                    string msg = string.Format("Cannot create new solution. The target solution folder '{0}' allready exists.", targetSolutionFolder);
                    _logger.Error(msg);
                    MessageBox.Show(msg, _solutionCreatorApplicationInfo.Name + " " + _solutionCreatorApplicationInfo.Version);
                    ExitCode = 1;
                    return;
                }
                _logger.InfoFormat("Creating new solution by resolving template folder '{0}' to new solution folder '{1}'.", selectedSolutionTemplate.Path, targetSolutionFolder);
                _folderResolver.Resolve(selectedSolutionTemplate.Path, targetSolutionFolder);
            }
            else
            {
                _logger.Warn("User canceled the operation of filling out solution info attributes.");
                ExitCode = 1;
                return;
            }      
            ExitCode = 0;      
            return;
        }

        private string GetSolutionName()
        {
            var solutionName = _resolveContext.GetVariable("_S_ShortProductName_S_");
            if(solutionName == null)
            {
                throw new SolutionCreatorExceptions("Was not able to get solution name from short product name. Solution attribute _S_ShortProductName_S_ must be specified.");
            }
            return solutionName;
        }

        private SolutionTemplate SelectSolutionTemplate()
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
                var dialogResult = window.ShowDialog();
                if(dialogResult == DialogResult.Ok)
                {
                    return _typeMapper.Map<SolutionTemplate>(viewModel.SelectedSolutionTemplate);
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                _selectSolutionTemplateWindowFactory.Release(window);
                this.ShutdownMode = ShutdownMode.OnLastWindowClose;
            }            
        }

        private CreateAction FillInSolutionInfoAttributes(SolutionTemplate solutionTemplate)
        {
            SolutionInfoWindow window = null;
            try
            {
                window = _solutionInfoWindowFactory.GetSolutionInfoWindow();
                var view = window.View;
                var viewModel = view.ViewModel;
                var solutionInfoAttributes =
                    _solutionInfoAttributeProvider.GetSolutionInfoAttributesFromTemplateFolder(solutionTemplate.Path);
                foreach (var solutionInfoAttribute in solutionInfoAttributes)
                {
                    _logger.Info(solutionInfoAttribute.Name);
                    var solutionInfoAttributeViewModel = _typeMapper.Map<SolutionInfoAttributeViewModel>(solutionInfoAttribute);
                    viewModel.SolutionInfoAttributes.Add(solutionInfoAttributeViewModel);
                    solutionInfoAttributeViewModel.MemberOfSolutionInfoAttributes = viewModel.SolutionInfoAttributes;
                }
                window.Activate();
                var dialogResult = window.ShowDialog();
                if (dialogResult == DialogResult.Ok)
                {
                    TransferSolutionInfoIntoResolveContext(viewModel.SolutionInfoAttributes);
                    return CreateAction.DoCreateSolution;                    
                }                
                return CreateAction.DoNotCreateSolution;                    

            }
            finally
            {
                _solutionInfoWindowFactory.Release(window);
            }
        }

        private enum CreateAction
        {
            DoCreateSolution,
            DoNotCreateSolution
        }

        private void TransferSolutionInfoIntoResolveContext(ObservableCollection<SolutionInfoAttributeViewModel> solutionInfoAttributes)
        {
            foreach (var solutionInfoAttributeViewModel in solutionInfoAttributes)
            {
                var solutionAttribute = _typeMapper.Map<SolutionInfoAttribute>(solutionInfoAttributeViewModel);
                AddSolutionInfoAttributeToResolverContext(solutionAttribute);
            }
        }

        private void AddSolutionInfoAttributeToResolverContext(SolutionInfoAttribute solutionAttribute)
        {
            _resolveContext.AddVariable(solutionAttribute.Name, solutionAttribute.Value);            
        }
        
        public void InitializeAndRun(string targetRootFolder)
        {
            if (targetRootFolder == null) throw new ArgumentNullException(nameof(targetRootFolder));
            _targetRootFolder = targetRootFolder;
            base.Run();
        }

        public int ExitCode { get; set; }
    }
}