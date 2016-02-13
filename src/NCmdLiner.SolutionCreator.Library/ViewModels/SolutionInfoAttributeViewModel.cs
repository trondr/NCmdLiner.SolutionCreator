using System.Windows;

namespace NCmdLiner.SolutionCreator.Library.ViewModels
{
    public class SolutionInfoAttributeViewModel : DependencyObject
    {
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name", typeof (string), typeof (SolutionInfoAttributeViewModel), new PropertyMetadata(default(string)));

        public string Name
        {
            get { return (string) GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof (string), typeof (SolutionInfoAttributeViewModel), new FrameworkPropertyMetadata(string.Empty,FrameworkPropertyMetadataOptions.AffectsMeasure,PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var viewModel = (SolutionInfoAttributeViewModel)dependencyObject;
            if (dependencyPropertyChangedEventArgs.NewValue != null)
            {
                viewModel.IsFilledOut = !string.IsNullOrWhiteSpace(dependencyPropertyChangedEventArgs.NewValue.ToString());
            }
            else
            {
                viewModel.IsFilledOut = false;
            }            
        }

        public string Value
        {
            get { return (string) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof (bool), typeof (SolutionInfoAttributeViewModel), new PropertyMetadata(default(bool)));

        public bool IsSelected
        {
            get { return (bool) GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsFilledOutProperty = DependencyProperty.Register(
            "IsFilledOut", typeof (bool), typeof (SolutionInfoAttributeViewModel), new PropertyMetadata(default(bool)));

        public bool IsFilledOut
        {
            get { return (bool) GetValue(IsFilledOutProperty); }
            set { SetValue(IsFilledOutProperty, value); }
        }

        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(
            "DisplayName", typeof (string), typeof (SolutionInfoAttributeViewModel), new PropertyMetadata(default(string)));

        public string DisplayName
        {
            get { return (string) GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }
    }
}