using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ChGK.Core.Services.Deserialization;

namespace ChGK.Core.Services.Rest
{
    public class RestService : IRestService
    {
        public async Task<T> GetAsync<T>(string host, string uri, IDeserializer<T> deserializer,
            CancellationToken cancellationToken) where T : class, new()
        {
            cancellationToken.ThrowIfCancellationRequested();

            var response = await Load(host, uri);

            cancellationToken.ThrowIfCancellationRequested();

            var toReturn = await deserializer.Deserialize(response);

            cancellationToken.ThrowIfCancellationRequested();

            return toReturn;
        }

        private async Task<string> Load(string host, string uri)
        {
            var httpClient = new HttpClient {BaseAddress = new Uri(host)};
            httpClient.DefaultRequestHeaders.Add("Accept", "application/xml");

            return await httpClient.GetStringAsync(uri);
        }
    }
}