using System.Threading.Tasks;

namespace ChGK.Core.Services.Deserialization
{
    public interface IDeserializer<T>
    {
        Task<T> Deserialize(string some);
    }
}