using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Common.Logging;
using NCmdLiner.SolutionCreator.Library.BootStrap;
using NCmdLiner.SolutionCreator.Library.Common.UI;

namespace NCmdLiner.SolutionCreator.Library.ViewModels
{
    public class SolutionInfoViewModel : DependencyObject, ISolutionInfoViewModel
    {
        private readonly ILog _logger;

        public SolutionInfoViewModel(ISolutionCreatorApplicationInfo solutionCreatorApplicationInfo, ILog logger)
        {
            _logger = logger;
            SolutionInfoAttributes = new ObservableCollection<SolutionInfoAttributeViewModel>();
            SolutionInfoAttributes.CollectionChanged += SolutionInfoAttributes_CollectionChanged; ;            
            ApplicationInfo = solutionCreatorApplicationInfo.Name + " " + solutionCreatorApplicationInfo.Version;
            SolutionInfoLabel = "Solution Attributes:";
            OkCommand = new AsyncCommand(Ok,() => !IsBusy );
            CancelCommand = new AsyncCommand(Cancel,() => !IsBusy);

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

        private void SolutionInfoAttributes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DispatchService.Invoke(() =>
            {
                IsBusy = true;
                UpdateView?.Invoke();
                IsBusy = false;
            });
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
        
        public string SolutionInfoLabel { get; set; }
        public ObservableCollection<SolutionInfoAttributeViewModel> SolutionInfoAttributes { get; set; }
        public bool IsBusy { get; set; }
        public Action<bool> CloseWindow { get; set; }
        public Action UpdateView { get; set; }
        public ICommand OkCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public string ApplicationInfo { get; set; }
    }
}