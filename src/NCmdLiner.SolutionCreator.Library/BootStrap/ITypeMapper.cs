using System.Security.Cryptography.X509Certificates;

namespace NCmdLiner.SolutionCreator.Library.BootStrap
{
    public interface ITypeMapper
    {
        T Map<T>(object source);
    }
}
