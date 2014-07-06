using System;
using System.Net;
using System.Xml;
using System.IO;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using System.Threading.Tasks;
using System.Net.Http;

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

