using System.Threading.Tasks;

namespace ChGK.Core.NetworkService
{
	public interface ISimpleRestService
	{
		Task<T> GetAsync<T> (string host, string uri, IDeserializer<T> deserializer) where T : class, new();
	}
}

