using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChGK.Core.NetworkService
{
	public class SimpleRestService : ISimpleRestService
	{
		async Task<string> Load (string host, string uri)
		{
			var httpClient = new HttpClient  { BaseAddress = new Uri (host) };
			httpClient.DefaultRequestHeaders.Add ("Accept", "application/xml");

			return await httpClient.GetStringAsync (uri);
		}

		public async Task<T> GetAsync<T> (string host, string uri, IDeserializer<T> deserializer) where T : class, new()
		{
			string response = await Load (host, uri);
			T toReturn = await deserializer.Deserialize (response);

			return toReturn;
		}
	}
}

