using System;

namespace NCmdLiner.SolutionCreator.Library.Services
{
    public class GuidGeneator : IGuidGeneator
    {
        public string GetNewGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}