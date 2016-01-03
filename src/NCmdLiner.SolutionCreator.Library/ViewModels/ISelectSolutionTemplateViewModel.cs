using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NCmdLiner.SolutionCreator.Library.ViewModels
{
    public interface ISelectSolutionTemplateViewModel
    {
        string SelectSolutionTemplateLabel { get; set; }

        ObservableCollection<SolutionTemplateViewModel> SolutionTemplates { get;set;}
        
        bool IsBusy {get;set; }

        Action<bool> CloseWindow {get;set; }

        Action UpdateView { get; set; }

        ICommand OkCommand { get; set; }

        ICommand CancelCommand { get; set; }

        string ApplicationInfo {get;set; }

        SolutionTemplateViewModel SelectedSolutionTemplate { get; }
    }
}