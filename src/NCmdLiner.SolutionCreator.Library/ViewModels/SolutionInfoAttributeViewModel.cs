using System.Windows;

namespace NCmdLiner.SolutionCreator.Library.ViewModels
{
    public class SolutionInfoAttributeViewModel : DependencyObject
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsSelected { get; set; }
    }
}