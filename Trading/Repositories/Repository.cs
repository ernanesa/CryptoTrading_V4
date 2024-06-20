using System.Net.Http.Headers;

namespace Trading.Repositories
{
    public class Repository
    {
        public readonly HttpClient _httpClient;

        public Repository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(configuration["ApiUrlMercadobitcoinV4"]);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}