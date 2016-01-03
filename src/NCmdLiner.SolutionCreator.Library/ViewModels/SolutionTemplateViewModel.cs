using System.Windows;

namespace NCmdLiner.SolutionCreator.Library.ViewModels
{
    public class SolutionTemplateViewModel: DependencyObject, ISolutionTemplateViewModel
    {
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name", typeof (string), typeof (SolutionTemplateViewModel), new PropertyMetadata(default(string)));

        public string Name
        {
            get { return (string) GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register(
            "Path", typeof (string), typeof (SolutionTemplateViewModel), new PropertyMetadata(default(string)));

        public string Path
        {
            get { return (string) GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof (bool), typeof (SolutionTemplateViewModel), new PropertyMetadata(default(bool)));

        public bool IsSelected
        {
            get { return (bool) GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }        
    }
}