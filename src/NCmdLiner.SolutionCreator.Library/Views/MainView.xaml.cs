using NCmdLiner.SolutionCreator.Library.ViewModels;

namespace NCmdLiner.SolutionCreator.Library.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : ViewBase
    {
        public MainViewModel ViewModel { get; set; }

        public MainView()
        {            
            InitializeComponent();
        }
    }
}
