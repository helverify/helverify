using System.Text;
using System.Text.Json;

namespace Helverify.VotingAuthority.DataAccess.Rest
{
    /// <inheritdoc cref="IRestClient"/>
    internal class RestClient : IRestClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpClientFactory">Client factory that allows us to instantiate HTTP clients</param>
        public RestClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <inheritdoc cref="IRestClient.Call{T}"/>
        public async Task<T?> Call<T>(HttpMethod method, Uri endpoint, object? body = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, endpoint);
            
            request.Headers.Add("Accept", "application/json");

            if (body != null)
            {
                string jsonBody = JsonSerializer.Serialize(body);

                request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            }
            
            HttpClient client = _httpClientFactory.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.StatusCode.ToString());
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            
            T? result = JsonSerializer.Deserialize<T>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return result;
        }

        /// <inheritdoc cref="IRestClient.Call"/>
        public async Task Call(HttpMethod method, Uri endpoint, object? body = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, endpoint);

            request.Headers.Add("Accept", "application/json");

            if (body != null)
            {
                string jsonBody = JsonSerializer.Serialize(body);

                request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            }

            HttpClient client = _httpClientFactory.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }
    }
}
