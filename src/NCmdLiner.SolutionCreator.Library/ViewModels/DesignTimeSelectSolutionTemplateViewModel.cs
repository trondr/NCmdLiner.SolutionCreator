using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;

namespace NCmdLiner.SolutionCreator.Library.ViewModels
{
    public class DesignTimeSelectSolutionTemplateViewModel : ISelectSolutionTemplateViewModel
    {
        public DesignTimeSelectSolutionTemplateViewModel()
        {
            SelectSolutionTemplateLabel = "Select solution template:";
            SolutionTemplates = new ObservableCollection<SolutionTemplateViewModel>()
            {
                new SolutionTemplateViewModel() {Name = "Console Application", Path = @"c:\temp\Templates\Console Application"},
                new SolutionTemplateViewModel() {Name = "Client Service Application", Path = @"c:\temp\Templates\Client Service Application"},
            };
            ApplicationInfo = "NCmdLiner SolutionCreator 1.0.0.0";
        }

        public string SelectSolutionTemplateLabel { get; set; }
        public ObservableCollection<SolutionTemplateViewModel> SolutionTemplates { get; set; }
        public bool IsBusy { get; set; }
        public Action<bool> CloseWindow { get; set; }
        public Action UpdateView { get; set; }
        public string ApplicationInfo { get; set; }
        public SolutionTemplateViewModel SelectedSolutionTemplate { get; }
        public ICommand OkCommand { get; set; }
        public ICommand CancelCommand { get; set; }
    }
}