using System;

namespace NCmdLiner.SolutionCreator.Library.Views
{
    public class SolutionCreatorExceptions : Exception
    {
        public SolutionCreatorExceptions(string message): base(message)
        {            
        }
    }
}