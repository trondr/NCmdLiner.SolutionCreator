using System;
using System.Windows;
using Common.Logging;
using NCmdLiner.SolutionCreator.Library.Common.UI;

namespace NCmdLiner.SolutionCreator.Library.Views
{
    /// <summary>
    /// Interaction logic for SolutionInfoWindow.xaml
    /// </summary>
    public partial class SolutionInfoWindow : Window
    {
        public ILog Logger { get; set; }

        public SolutionInfoView View { get; set; }

        public SolutionInfoWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            Logger.Debug(this.GetType().FullName + " is loaded.");
            if(this.View == null) throw new NullReferenceException("View has not been initialized. Has view been registered with the container?");
            this.View.ViewModel.CloseWindow = dialogResult =>
            {
                if(this.IsModal())
                {
                    this.DialogResult = dialogResult;
                }
                this.Close();
            };
            this.View.ViewModel.UpdateView = new Action(() => {ResetColumnWidthsGridView.ResetColumnWidts(this.View._solutionInfoAttributesGridView);});
            this.View.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.View.VerticalAlignment = VerticalAlignment.Stretch;            
            if (WindowDockPanel.Children.Count == 0)
                WindowDockPanel.Children.Add(this.View);
        }
    }
}
