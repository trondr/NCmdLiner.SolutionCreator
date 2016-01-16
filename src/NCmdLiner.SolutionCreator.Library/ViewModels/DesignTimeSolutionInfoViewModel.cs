using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace NCmdLiner.SolutionCreator.Library.ViewModels
{
    public class DesignTimeSolutionInfoViewModel : DependencyObject, ISolutionInfoViewModel
    {
        public DesignTimeSolutionInfoViewModel()
        {
            SolutionInfoLabel = "Fill out solution attributes:";
            SolutionInfoAttributes = new ObservableCollection<SolutionInfoAttributeViewModel>()
            {
                new SolutionInfoAttributeViewModel() {Name="Console Project Name", Value = "MyProject"},
                new SolutionInfoAttributeViewModel() {Name="Library Project Name", Value = "MyProject.Library"},
                new SolutionInfoAttributeViewModel() {Name="Setup Project Name", Value = "MyProject.Setup"}
            };
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