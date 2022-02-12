using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using ErgoNames.Api.Models.Configuration;
using ErgoNames.Api.Services.Interfaces;

namespace ErgoNames.Api.Services
{
    public class ErgoExplorerClient : IErgoExplorerClient
    {
        private const string MAINNET_EXPLORER_URL = "https://api.ergoplatform.com/api/v1/";
        private const string TESTNET_EXPLORER_URL = "https://api-testnet.ergoplatform.com/api/v1/";

        private readonly HttpClient explorerApiClient;

        public ErgoExplorerClient(ErgoNameApiConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            explorerApiClient = httpClientFactory.CreateClient();
            explorerApiClient.BaseAddress = configuration.NetworkType == NetworkType.Mainnet ? 
                new Uri(MAINNET_EXPLORER_URL) : 
                new Uri(TESTNET_EXPLORER_URL);
            
            explorerApiClient.DefaultRequestHeaders.Accept.Clear();
            explorerApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<T> GetAsync<T>(string queryString)
        {
            var response = await explorerApiClient.GetAsync(queryString);
            var serializedResponseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<T>(serializedResponseContent);
            return result;
        }
    }
}
