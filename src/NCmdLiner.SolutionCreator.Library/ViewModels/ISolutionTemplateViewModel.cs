namespace NCmdLiner.SolutionCreator.Library.ViewModels
{
    public interface ISolutionTemplateViewModel
    {
        string Name {get;set; }

        string Path {get;set; }

        bool IsSelected { get; set; }
    }    
}