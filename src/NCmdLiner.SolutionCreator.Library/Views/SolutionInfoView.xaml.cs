using System;
using NCmdLiner.SolutionCreator.Library.ViewModels;

namespace NCmdLiner.SolutionCreator.Library.Views
{
    /// <summary>
    /// Interaction logic for SolutionInfoView.xaml
    /// </summary>
    public partial class SolutionInfoView : ViewBase
    {
        public SolutionInfoViewModel ViewModel {get; set; }

        public SolutionInfoView()
        {
            InitializeComponent();
            Loaded += UserControlLoaded;
        }

        private void UserControlLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if(this.ViewModel == null) throw new NullReferenceException("ViewModel has not been initialized. Has ViewModel component been registered with the container?");
            this.DataContext = ViewModel;
        }
    }
}
