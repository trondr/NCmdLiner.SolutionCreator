namespace NCmdLiner.SolutionCreator.Library.Services
{
    public class CheckFileExtentions : ICheckFileExtentions
    {
        public bool IsBinaryFileExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".dll": return true;
                case ".exe": return true;
                case ".baml": return true;
                case ".bmp": return true;
                case ".cache": return true;
                case ".chm": return true;
                case ".zip": return true;
                case ".gif": return true;
                case ".ico": return true;
                case ".jpg": return true;
                case ".nupkg": return true;
                case ".png": return true;
                case ".pdb": return true;
                case ".resources": return true;
                case ".msi": return true;
                case ".wixobj": return true;
                case ".xcf": return true;
                case ".suo": return true;
                default:
                    return false;
            }
        }
    }
}