namespace NCmdLiner.SolutionCreator.Library.Services
{
    public interface ISolutionAttributeHelper
    {
        SolutionInfoAttribute GetSolutionInfoAttributeFromAttributeName(string solutionInfoAttributeName);
    }
}