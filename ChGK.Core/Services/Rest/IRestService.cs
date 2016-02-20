using System.Threading;
using System.Threading.Tasks;
using ChGK.Core.Services.Deserialization;

namespace ChGK.Core.Services.Rest
{
    public interface IRestService
    {
        Task<T> GetAsync<T>(string host, string uri, IDeserializer<T> deserializer, CancellationToken cancellationToken)
            where T : class, new();
    }
}