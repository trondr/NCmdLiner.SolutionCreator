using System.Collections.Generic;
using System.IO;

namespace NCmdLiner.SolutionCreator.Library.Common.IO
{
    public interface IIniFileOperation
    {
        string Read(string path, string section, string key);

        void Write(string path, string section, string key, string value);

        IEnumerable<string> GetKeys(string path, string section);

        IEnumerable<string> GetSections(string path);
    }
}