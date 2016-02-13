using System.Collections.ObjectModel;
using System.Windows;

namespace NCmdLiner.SolutionCreator.Library.ViewModels
{
    public class SolutionInfoAttributeViewModel : DependencyObject
    {
        public SolutionInfoAttributeViewModel()
        {
            UpdateOtherSolutionAttributes = true;
        }
        
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
            viewModel.IsFilledOut = NewValueIsFilledOut(dependencyPropertyChangedEventArgs.NewValue);
            if(viewModel.UpdateOtherSolutionAttributes)
            {
                UpdateOtherSolutionAttributes2(viewModel.Name,dependencyPropertyChangedEventArgs.OldValue,dependencyPropertyChangedEventArgs.NewValue, viewModel.MemberOfSolutionInfoAttributes);
            }
        }

        private static void UpdateOtherSolutionAttributes2(string name, object oldValue, object newValue, ObservableCollection<SolutionInfoAttributeViewModel> memberOfSolutionInfoAttributes)
        {
            if(oldValue == null) return;
            if(newValue == null) return;
            if(memberOfSolutionInfoAttributes!=null)
            {
                foreach (var solutionInfoAttributeViewModel in memberOfSolutionInfoAttributes)
                {
                    if(solutionInfoAttributeViewModel.Name == name) continue; 
                    solutionInfoAttributeViewModel.UpdateOtherSolutionAttributes = false;               
                    solutionInfoAttributeViewModel.Value =  solutionInfoAttributeViewModel.Value.Replace(oldValue.ToString(),newValue.ToString());
                    solutionInfoAttributeViewModel.UpdateOtherSolutionAttributes = true;
                }
            }
        }
        
        private static bool NewValueIsFilledOut(object newValue)
        {
            return !string.IsNullOrWhiteSpace(newValue?.ToString());
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

        public ObservableCollection<SolutionInfoAttributeViewModel> MemberOfSolutionInfoAttributes { get; set; }


        public static readonly DependencyProperty UpdateOtherSolutionAttributesProperty = DependencyProperty.Register(
            "UpdateOtherSolutionAttributes", typeof (bool), typeof (SolutionInfoAttributeViewModel), new PropertyMetadata(default(bool)));

        public bool UpdateOtherSolutionAttributes
        {
            get { return (bool) GetValue(UpdateOtherSolutionAttributesProperty); }
            set { SetValue(UpdateOtherSolutionAttributesProperty, value); }
        }
    }
}