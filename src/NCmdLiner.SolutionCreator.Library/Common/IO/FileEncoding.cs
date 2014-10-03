using System.IO;
using System.Text;

namespace NCmdLiner.SolutionCreator.Library.Common.IO
{
    public class FileEncoding : IFileEncoding
    {
        public Encoding GetEncoding(string fileName)
        {
            int bom0, bom1, bom2, bom3;
            using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                bom0 = fileStream.ReadByte();
                bom1 = fileStream.ReadByte();
                bom2 = fileStream.ReadByte();
                bom3 = fileStream.ReadByte();
            }
            
            if (bom0 == 0xef && bom1 == 0xbb && bom2 == 0xbf) return Encoding.UTF8;
            if (bom0 == 0x2b && bom1 == 0x2f && bom2 == 0x78) return Encoding.UTF7;
            if (bom0 == 0xff && bom1 == 0xfe && bom2 == 0 && bom3 == 0) return Encoding.UTF32;
            if (bom0 == 0xfe && bom1 == 0xff && bom2 == 0) return Encoding.BigEndianUnicode;
            if (bom0 == 0xff && bom1 == 0xfe) return Encoding.Unicode;
            if (bom0 == 0 && bom1 == 0 && bom2 == 0xfe && bom3 == 0xff) return Encoding.UTF32;            
            return Encoding.Default;
        }
    }
}