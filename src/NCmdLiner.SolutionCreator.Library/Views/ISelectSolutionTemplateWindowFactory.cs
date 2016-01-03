namespace NCmdLiner.SolutionCreator.Library.Views
{
    public interface ISelectSolutionTemplateWindowFactory
    {
        SelectSolutionTemplateWindow GetSelectSolutionTemplateWindow();

        void Release(SelectSolutionTemplateWindow selectSolutionTemplateWindow);
    }
}
