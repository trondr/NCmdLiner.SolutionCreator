using System;
using System.Windows;
using System.Windows.Controls;
using NCmdLiner.SolutionCreator.Library.ViewModels;

namespace NCmdLiner.SolutionCreator.Library.Views
{
    public class ViewBase: UserControl
    {
        protected ViewBase()
        {
            Loaded+=OnLoaded;
        }

        public ViewModelBase ViewModel { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            this.DataContext = this.ViewModel;
        }
    }
}
