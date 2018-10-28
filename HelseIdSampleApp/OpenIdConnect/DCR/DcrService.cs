using HelseIdSampleApp.OpenIdConnect.Clients;
using HelseIdSampleApp.OpenIdConnect.DCR.Client;
using HelseIdSampleApp.OpenIdConnect.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HelseIdSampleApp.OpenIdConnect.DCR.Api
{
    public class DcrService
    {
        private readonly DcrClient client;
        private string dcrApiEndpoint;

        public DcrService(DcrServiceSettings settings)
        {
            dcrApiEndpoint = settings.DcrApi;
            client = new DcrClient(settings.DcrApi);
        }

        public async Task<ClientResponse> CreateClient(string accessToken, string grantType, string redirectUri, string logoutUri, string[] allowedScopes, string organizationName, string organizationEnhId)
        {
            var grantTypes = grantType == null ? null : new List<string>(grantType.Split(' '));

            return await CreateClient(accessToken, grantTypes, new List<string> { redirectUri }, logoutUri, allowedScopes, organizationEnhId, organizationName);
        }

        public async Task<ClientResponse> CreateClient(string accessToken, List<string> grantTypes, List<string> redirectUris, string logoutUri, string[] allowedScopes, string organizationName, string organizationEnhId)
        {
            var clientRequest = new ClientRequest
            {
                ClientName = $"{organizationName} ({organizationEnhId})",
                Secrets = new[] 
                {
                    new Secret
                    {
                        Type = SecretTypes.RsaPrivateKey,
                        Value = RSAKeyGenerator.CreateNewKey(false, organizationEnhId)
                    }
                },
                RequireClientSecret = false,
                AlwaysSendClientClaims = true,
                GrantTypes = grantTypes,
                RedirectUris = redirectUris,
                LogoutUri = logoutUri,
                AllowedScopes = allowedScopes,
                AllowAccessTokensViaBrowser = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                AllowOfflineAccess = true
            };
            
            client.SetBearerToken(accessToken);
            return await client.StoreClient(clientRequest);
        }

        public void SetOrgNumber(string accessToken, string clientId, string organizationEnhId)
        {
            var httpClient = new HttpClient { BaseAddress = new Uri(dcrApiEndpoint) };
            httpClient.SetBearerToken(accessToken);

            var vm = new OrgNrViewModel
            {
                ClientId = clientId,
                Orgnr = organizationEnhId
            };

            var content = new StringContent(JsonConvert.SerializeObject(vm), Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync("api/kjorgnr/", content);
            var result = response.GetAwaiter().GetResult();

            if (result == null)
            {
                throw new ArgumentNullException("No response from HelseId while setting the org number");
            }

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(result.ReasonPhrase);
            }
        }
    }

    public class OrgNrViewModel
    {
        public string Orgnr { get; set; }
        public string ClientId { get; set; }
    }

    public class DcrServiceSettings
    {
        public const string DefaultGrantTypes = "client_credentials authorization_code";

        public string Authority { get; set; }

        public string DcrApi { get; set; }

        public string Thumbprint { get; set; }

        public string ClientId { get; set; }

        public string RedirectUri { get; set; }

        public string LogoutUri { get; set; }

        public string Scopes { get; set; }
    }
}
