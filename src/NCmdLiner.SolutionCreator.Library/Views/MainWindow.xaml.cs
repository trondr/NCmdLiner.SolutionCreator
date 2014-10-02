using System;
using System.Windows;
using Common.Logging;
using NCmdLiner.SolutionCreator.Library.Common;

namespace NCmdLiner.SolutionCreator.Library.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ILog Logger { get; set; }

        public MainView View { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();
            this.Title = ApplicationInfoHelper.ApplicationName + " " + ApplicationInfoHelper.ApplicationVersion;
            Loaded += OnLoaded;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            Logger.Debug("MainWindow is loaded.");
            if(this.View == null) throw new NullReferenceException("View has not been initialized");
            if (MainWindowDockPanel.Children.Count == 0)
                MainWindowDockPanel.Children.Add(this.View);
        }
    }
}
