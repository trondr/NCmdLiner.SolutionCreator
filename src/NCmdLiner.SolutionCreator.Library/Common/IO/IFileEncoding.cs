using System.Text;

namespace NCmdLiner.SolutionCreator.Library.Common.IO
{
    public interface IFileEncoding
    {
        Encoding GetEncoding(string fileName);
    }
}
