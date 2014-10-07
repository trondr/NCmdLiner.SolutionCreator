using System;
using System.IO;
using NCmdLiner.SolutionCreator.Library.Common;

namespace NCmdLiner.SolutionCreator.Library.Services
{
    public class TemplatePath : ITemplatePath
    {
        public string GetFullPath(string path)
        {
            var fullPath = path;
            if (path.StartsWith(@".\"))
            {
                var directoryInfo = new FileInfo(typeof(TemplatePath).Assembly.Location).Directory;
                if (directoryInfo != null)
                {
                    var exeDirectory = directoryInfo.FullName;
                    fullPath = Path.Combine(exeDirectory, path.Substring(2));
                }
            }
            fullPath = Path.GetFullPath(Environment.ExpandEnvironmentVariables(fullPath));
            return fullPath;
        }
    }
}