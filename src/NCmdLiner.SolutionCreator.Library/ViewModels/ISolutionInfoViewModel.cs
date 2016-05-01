using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NCmdLiner.SolutionCreator.Library.ViewModels
{
    public interface ISolutionInfoViewModel
    {
        ObservableCollection<SolutionInfoAttributeViewModel> SolutionInfoAttributes { get;set;}
        
        bool IsBusy {get;set; }

        Action<bool> CloseWindow {get;set; }

        Action UpdateView { get; set; }

        ICommand OkCommand { get; set; }

        ICommand CancelCommand { get; set; }

        string ApplicationInfo {get;set; }

        bool AllAttributesAreFilledOut { get; }

        bool EnableMultiEdit {get;set; }
    }
}