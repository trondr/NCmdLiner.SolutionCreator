using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Common.Logging;
using NCmdLiner.SolutionCreator.Library.BootStrap;
using NCmdLiner.SolutionCreator.Library.Common.UI;

namespace NCmdLiner.SolutionCreator.Library.ViewModels
{
    public class SelectSolutionTemplateViewModel : DependencyObject, ISelectSolutionTemplateViewModel
    {
        private readonly ILog _logger;
        
        public SelectSolutionTemplateViewModel(ISolutionCreatorApplicationInfo solutionCreatorApplicationInfo, ILog logger)
        {
            _logger = logger;
            SolutionTemplates = new ObservableCollection<SolutionTemplateViewModel>();
            SolutionTemplates.CollectionChanged += SolutionTemplatesOnCollectionChanged;            
            ApplicationInfo = solutionCreatorApplicationInfo.Name + " " + solutionCreatorApplicationInfo.Version;
            SelectSolutionTemplateLabel = "Select Solution Template:";
            OkCommand = new AsyncCommand(Ok,() => !IsBusy && SelectedSolutionTemplate != null);
            CancelCommand = new AsyncCommand(Cancel,() => !IsBusy);            
        }

        private void SolutionTemplatesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            DispatchService.Invoke(() =>
            {
                IsBusy = true;
                UpdateView?.Invoke();
                IsBusy = false;
            });
        }

        private Task Ok()
        {
            return Task.Factory.StartNew(() => DispatchService.Invoke(() =>
            {                
                CloseWindowWithDialogResult(DialogResult.Ok);                
            }));
        }

        private Task Cancel()
        {
            return Task.Factory.StartNew(() => DispatchService.Invoke(() =>
            {                
                CloseWindowWithDialogResult(DialogResult.Cancel);                
            }));
        }

        private void CloseWindowWithDialogResult(bool dialogResult)
        {
            try
            {
                IsBusy = true;
                if (CloseWindow != null)
                {                    
                    CloseWindow(dialogResult);
                }
                else
                {
                    _logger.Error("CloseWindow action has not been set.");
                }
            }
            catch(Exception ex)
            {
                _logger.ErrorFormat("Failed to close window. {0}{1}", Environment.NewLine, ex.ToString());
            }
            finally
            {
                IsBusy = false;
            }
        }

        public static readonly DependencyProperty SelectSolutionTemplateLabelProperty = DependencyProperty.Register(
            "SelectSolutionTemplateLabel", typeof (string), typeof (SelectSolutionTemplateViewModel), new PropertyMetadata(default(string)));
        
        public string SelectSolutionTemplateLabel
        {
            get { return (string) GetValue(SelectSolutionTemplateLabelProperty); }
            set { SetValue(SelectSolutionTemplateLabelProperty, value); }
        }

        public ObservableCollection<SolutionTemplateViewModel> SolutionTemplates { get; set; }

        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
            "IsBusy", typeof (bool), typeof (SelectSolutionTemplateViewModel), new PropertyMetadata(default(bool)));

        public bool IsBusy
        {
            get { return (bool) GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public Action<bool> CloseWindow { get; set; }
        public Action UpdateView { get; set; }

        public static readonly DependencyProperty ApplicationInfoProperty = DependencyProperty.Register(
            "ApplicationInfo", typeof (string), typeof (SelectSolutionTemplateViewModel), new PropertyMetadata(default(string)));

        public string ApplicationInfo
        {
            get { return (string) GetValue(ApplicationInfoProperty); }
            set { SetValue(ApplicationInfoProperty, value); }
        }

        public SolutionTemplateViewModel SelectedSolutionTemplate
        {
            get
            {
                return SolutionTemplates.FirstOrDefault(p => p.IsSelected);
            }
        }

        public ICommand OkCommand { get; set; }
        public ICommand CancelCommand { get; set; }
    }
}