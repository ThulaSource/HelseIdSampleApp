using HelseIdSampleApp.OpenIdConnect.DCR.Api;
using HelseIdSampleApp.OpenIdConnect.DCR.Client;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HelseIdSampleApp.OpenIdConnect.Clients
{
    public class DcrClient
    {
        private HttpClient client;

        public DcrClient(string uri, string accessToken = "")
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };

            if (!string.IsNullOrEmpty(accessToken))
            {
                client.SetBearerToken(accessToken);
            }
        }

        public void SetBearerToken(string accessToken)
        {
            client.SetBearerToken(accessToken);
        }

        public async Task<ClientResponse> GetClient(string id)
        {
            var response = await client.GetAsync("api/connect/client/register?id=" + id);

            var responseAsJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ClientResponse>(responseAsJson);
        }

        public async Task<ClientResponse> StoreClient(ClientRequest request)
        {
            var requestAsJson = JsonConvert.SerializeObject(request);
            var content = new StringContent(requestAsJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/connect/client/register", content);

            var responseAsJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ClientResponse>(responseAsJson);
        }

        public async Task<ClientResponse> UpdateClient(ClientRequest request)
        {
            var requestAsJson = JsonConvert.SerializeObject(request);
            var content = new StringContent(requestAsJson, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("api/connect/client/register", content);

            var responseAsJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ClientResponse>(responseAsJson);
        }

        public async Task<ApiResourceResponse> GetApi(string id)
        {
            var response = await client.GetAsync("api/connect/apiResource/register?id=" + id);

            var responseAsJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiResourceResponse>(responseAsJson);
        }

        public async Task<ApiResourceResponse> StoreApi(ApiResourceRequest request)
        {
            var requestAsJson = JsonConvert.SerializeObject(request);
            var content = new StringContent(requestAsJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/connect/apiResource/register", content);

            var responseAsJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiResourceResponse>(responseAsJson);
        }

        public async Task<ApiResourceResponse> UpdateApi(ApiResourceRequest request)
        {
            var requestAsJson = JsonConvert.SerializeObject(request);
            var content = new StringContent(requestAsJson, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("api/connect/apiResource/register", content);

            var responseAsJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiResourceResponse>(responseAsJson);
        }
    }
}
