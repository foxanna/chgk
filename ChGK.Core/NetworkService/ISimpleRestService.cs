using System.Threading.Tasks;
using System.Threading;

namespace ChGK.Core.NetworkService
{
	public interface ISimpleRestService
	{
		Task<T> GetAsync<T> (string host, string uri, IDeserializer<T> deserializer, CancellationToken cancellationToken) where T : class, new();
	}
}

