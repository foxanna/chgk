using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

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

		public async Task<T> GetAsync<T> (string host, string uri, IDeserializer<T> deserializer, CancellationToken cancellationToken) where T : class, new()
		{
			cancellationToken.ThrowIfCancellationRequested ();

			string response = await Load (host, uri);

			cancellationToken.ThrowIfCancellationRequested ();

			T toReturn = await deserializer.Deserialize (response);

			cancellationToken.ThrowIfCancellationRequested ();

			return toReturn;
		}
	}
}

