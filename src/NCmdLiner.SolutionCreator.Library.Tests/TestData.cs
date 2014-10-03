using System.IO;
using System.Text;

namespace NCmdLiner.SolutionCreator.Library.Tests
{
    public class TestData
    {
        public static void CreateTestTextFile(string fileName, Encoding encoding, string text)
        {
            using (var sw = new StreamWriter(fileName, false, encoding))
            {
                sw.Write(text);
            }
        }

        public static string ReadTestTextFile(string fileName, Encoding encoding)
        {
            using (var sw = new StreamReader(fileName, encoding))
            {
                return sw.ReadToEnd();
            }
        }
    }
}
