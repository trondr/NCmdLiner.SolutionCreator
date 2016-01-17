using System;
using System.Collections.ObjectModel;
using System.Linq;
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
            OkCommand = new AsyncCommand(Ok, () => !IsBusy && AllAttributesAreFilledOut);
            CancelCommand = new AsyncCommand(Cancel, () => !IsBusy);
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
            catch (Exception ex)
            {
                _logger.ErrorFormat("Failed to close window. {0}{1}", Environment.NewLine, ex.ToString());
            }
            finally
            {
                IsBusy = false;
            }
        }

        public static readonly DependencyProperty SolutionInfoLabelProperty = DependencyProperty.Register(
            "SolutionInfoLabel", typeof(string), typeof(SolutionInfoViewModel), new PropertyMetadata(default(string)));

        public string SolutionInfoLabel
        {
            get { return (string)GetValue(SolutionInfoLabelProperty); }
            set { SetValue(SolutionInfoLabelProperty, value); }
        }


        public ObservableCollection<SolutionInfoAttributeViewModel> SolutionInfoAttributes { get; set; }

        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
            "IsBusy", typeof(bool), typeof(SolutionInfoViewModel), new PropertyMetadata(default(bool)));

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
        public Action<bool> CloseWindow { get; set; }
        public Action UpdateView { get; set; }
        public ICommand OkCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public static readonly DependencyProperty ApplicationInfoProperty = DependencyProperty.Register(
            "ApplicationInfo", typeof(string), typeof(SolutionInfoViewModel), new PropertyMetadata(default(string)));

        public string ApplicationInfo
        {
            get { return (string)GetValue(ApplicationInfoProperty); }
            set { SetValue(ApplicationInfoProperty, value); }
        }

        public bool AllAttributesAreFilledOut
        {
            get
            {
                return SolutionInfoAttributes.All(model => model.IsFilledOut);
            }
        }
    }
}