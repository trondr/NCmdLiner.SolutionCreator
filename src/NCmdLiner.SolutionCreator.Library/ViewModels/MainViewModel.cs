using System;
using System.Windows;
using System.Windows.Input;
using NCmdLiner.SolutionCreator.Library.Common.UI;
using NCmdLiner.SolutionCreator.Library.Views;

namespace NCmdLiner.SolutionCreator.Library.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public MainViewModel()
        {
            CompanyNameLabelText = "Company Name:";
            NamespaceCompanyNameLabelText = "Namespace Company Name:";
            ProductNameLabelText = "Product Name:";
            ShortProductNameLabelText = "Short Product Name:";
            ProductDescriptionLabelText = "Product Description:";
            ConsoleProjectNameLabelText = "Console Project Name:";
            LibraryProjectNameLabelText = "Library Project Name:";
            TestsProjectNameLabelText = "Tests Project Name:";
            SetupProjectNameLabelText = "Setup Project Name:";
            MaxLabelWidth = 200 ;
            OkCommand = new CommandHandler(this.Exit, true);
        }

        public static readonly DependencyProperty CompanyNameProperty = DependencyProperty.Register(
            "CompanyName", typeof(string), typeof(MainViewModel), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure, CompanyNameChanged));

        private static void CompanyNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = (MainViewModel)d;
            viewModel.NamespaceCompanyName = viewModel.CompanyName.Replace(" ", "");
            CommandManager.InvalidateRequerySuggested();
        }

        public string CompanyName
        {
            get { return (string)GetValue(CompanyNameProperty); }
            set { SetValue(CompanyNameProperty, value); }
        }

        public static readonly DependencyProperty CompanyNameLabelTextProperty = DependencyProperty.Register(
            "CompanyNameLabelText", typeof(string), typeof(MainViewModel), new PropertyMetadata(default(string)));

        public string CompanyNameLabelText
        {
            get { return (string)GetValue(CompanyNameLabelTextProperty); }
            set { SetValue(CompanyNameLabelTextProperty, value); }
        }

        public static readonly DependencyProperty NamespaceCompanyNameProperty = DependencyProperty.Register(
            "NamespaceCompanyName", typeof(string), typeof(MainViewModel), new PropertyMetadata(default(string)));

        public string NamespaceCompanyName
        {
            get { return (string)GetValue(NamespaceCompanyNameProperty); }
            set { SetValue(NamespaceCompanyNameProperty, value); }
        }

        public static readonly DependencyProperty NamespaceCompanyNameLabelTextProperty = DependencyProperty.Register(
            "NamespaceCompanyNameLabelText", typeof(string), typeof(MainViewModel), new PropertyMetadata(default(string)));

        public string NamespaceCompanyNameLabelText
        {
            get { return (string)GetValue(NamespaceCompanyNameLabelTextProperty); }
            set { SetValue(NamespaceCompanyNameLabelTextProperty, value); }
        }


        public static readonly DependencyProperty ProductNameProperty = DependencyProperty.Register(
            "ProductName", typeof(string), typeof(MainViewModel), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure, ProductNameChanged));

        private static void ProductNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = (MainViewModel)d;
            viewModel.ShortProductName = viewModel.ProductName.Replace(" ", ".");
            CommandManager.InvalidateRequerySuggested();
        }

        public string ProductName
        {
            get { return (string)GetValue(ProductNameProperty); }
            set { SetValue(ProductNameProperty, value); }
        }

        public static readonly DependencyProperty ProductNameLabelTextProperty = DependencyProperty.Register(
            "ProductNameLabelText", typeof(string), typeof(MainViewModel), new PropertyMetadata(default(string)));

        public string ProductNameLabelText
        {
            get { return (string)GetValue(ProductNameLabelTextProperty); }
            set { SetValue(ProductNameLabelTextProperty, value); }
        }

        public static readonly DependencyProperty ShortProductNameProperty = DependencyProperty.Register(
            "ShortProductName", typeof(string), typeof(MainViewModel), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure, ShortProductNameChanged));

        private static void ShortProductNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = (MainViewModel)d;
            viewModel.ConsoleProjectName = viewModel.NamespaceCompanyName + "." + viewModel.ShortProductName;
            CommandManager.InvalidateRequerySuggested();
        }

        public string ShortProductName
        {
            get { return (string)GetValue(ShortProductNameProperty); }
            set { SetValue(ShortProductNameProperty, value); }
        }

        public static readonly DependencyProperty ShortProductNameLabelTextProperty = DependencyProperty.Register(
            "ShortProductNameLabelText", typeof(string), typeof(MainViewModel), new PropertyMetadata(default(string)));

        public string ShortProductNameLabelText
        {
            get { return (string)GetValue(ShortProductNameLabelTextProperty); }
            set { SetValue(ShortProductNameLabelTextProperty, value); }
        }

        public static readonly DependencyProperty ProductDescriptionProperty = DependencyProperty.Register(
            "ProductDescription", typeof(string), typeof(MainViewModel), new PropertyMetadata(default(string)));

        public string ProductDescription
        {
            get { return (string)GetValue(ProductDescriptionProperty); }
            set { SetValue(ProductDescriptionProperty, value); }
        }

        public static readonly DependencyProperty ProductDescriptionLabelTextProperty = DependencyProperty.Register(
            "ProductDescriptionLabelText", typeof(string), typeof(MainViewModel), new PropertyMetadata(default(string)));

        public string ProductDescriptionLabelText
        {
            get { return (string)GetValue(ProductDescriptionLabelTextProperty); }
            set { SetValue(ProductDescriptionLabelTextProperty, value); }
        }

        public static readonly DependencyProperty ConsoleProjectNameProperty = DependencyProperty.Register(
            "ConsoleProjectName", typeof(string), typeof(MainViewModel), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure, ConsoleProjectNameChanged));

        private static void ConsoleProjectNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = (MainViewModel)d;
            viewModel.LibraryProjectName = viewModel.ConsoleProjectName + ".Library";
            viewModel.TestsProjectName = viewModel.ConsoleProjectName + ".Tests";
            viewModel.SetupProjectName = viewModel.ConsoleProjectName + ".Setup";
            CommandManager.InvalidateRequerySuggested();
        }

        public string ConsoleProjectName
        {
            get { return (string)GetValue(ConsoleProjectNameProperty); }
            set { SetValue(ConsoleProjectNameProperty, value); }
        }

        public static readonly DependencyProperty ConsoleProjectNameLabelTextProperty = DependencyProperty.Register(
            "ConsoleProjectNameLabelText", typeof(string), typeof(MainViewModel), new PropertyMetadata(default(string)));

        public string ConsoleProjectNameLabelText
        {
            get { return (string)GetValue(ConsoleProjectNameLabelTextProperty); }
            set { SetValue(ConsoleProjectNameLabelTextProperty, value); }
        }

        public static readonly DependencyProperty LibraryProjectNameProperty = DependencyProperty.Register(
            "LibraryProjectName", typeof(string), typeof(MainViewModel), new PropertyMetadata(default(string)));

        public string LibraryProjectName
        {
            get { return (string)GetValue(LibraryProjectNameProperty); }
            set { SetValue(LibraryProjectNameProperty, value); }
        }

        public static readonly DependencyProperty LibraryProjectNameLabelTextProperty = DependencyProperty.Register(
            "LibraryProjectNameLabelText", typeof(string), typeof(MainViewModel), new PropertyMetadata(default(string)));

        public string LibraryProjectNameLabelText
        {
            get { return (string)GetValue(LibraryProjectNameLabelTextProperty); }
            set { SetValue(LibraryProjectNameLabelTextProperty, value); }
        }

        public static readonly DependencyProperty TestsProjectNameProperty = DependencyProperty.Register(
            "TestsProjectName", typeof(string), typeof(MainViewModel), new PropertyMetadata(default(string)));

        public string TestsProjectName
        {
            get { return (string)GetValue(TestsProjectNameProperty); }
            set { SetValue(TestsProjectNameProperty, value); }
        }

        public static readonly DependencyProperty TestsProjectNameLabelTextProperty = DependencyProperty.Register(
            "TestsProjectNameLabelText", typeof(string), typeof(MainViewModel), new PropertyMetadata(default(string)));

        public string TestsProjectNameLabelText
        {
            get { return (string)GetValue(TestsProjectNameLabelTextProperty); }
            set { SetValue(TestsProjectNameLabelTextProperty, value); }
        }

        public static readonly DependencyProperty SetupProjectNameProperty = DependencyProperty.Register(
            "SetupProjectName", typeof(string), typeof(MainViewModel), new PropertyMetadata(default(string)));

        public string SetupProjectName
        {
            get { return (string)GetValue(SetupProjectNameProperty); }
            set { SetValue(SetupProjectNameProperty, value); }
        }

        public static readonly DependencyProperty SetupProjectNameLabelTextProperty = DependencyProperty.Register(
            "SetupProjectNameLabelText", typeof(string), typeof(MainViewModel), new PropertyMetadata(default(string)));

        public string SetupProjectNameLabelText
        {
            get { return (string)GetValue(SetupProjectNameLabelTextProperty); }
            set { SetValue(SetupProjectNameLabelTextProperty, value); }
        }

        public static readonly DependencyProperty MaxLabelWidthProperty = DependencyProperty.Register(
            "MaxLabelWidth", typeof(int), typeof(MainViewModel), new PropertyMetadata(default(int)));

        public int MaxLabelWidth
        {
            get { return (int)GetValue(MaxLabelWidthProperty); }
            set { SetValue(MaxLabelWidthProperty, value); }
        }

        public ICommand OkCommand { get; set; }

        private void Exit()
        {
            if (MainWindow != null)
            {
                MainWindow.Close();
            }
            else
            {
                throw new Exception("Unable to close main window as no reference to the main window has been set.");
            }
        }

        public bool IsFilledOut
        {
            get
            {
                return
                    !string.IsNullOrEmpty(this.CompanyName) &&
                    !string.IsNullOrEmpty(this.NamespaceCompanyName) &&
                    !string.IsNullOrEmpty(this.ProductName) &&
                    !string.IsNullOrEmpty(this.ShortProductName) &&
                    !string.IsNullOrEmpty(this.ProductDescription) &&
                    !string.IsNullOrEmpty(this.ConsoleProjectName) &&
                    !string.IsNullOrEmpty(this.LibraryProjectName) &&
                    !string.IsNullOrEmpty(this.TestsProjectName) &&
                    !string.IsNullOrEmpty(this.SetupProjectName);
            }
        }
    }
}