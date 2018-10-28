using HelseIdSampleApp.BrowserLogic;
using HelseIdSampleApp.Configuration;
using HelseIdSampleApp.OpenIdConnect.Helpers;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using System;
using static IdentityModel.OidcClient.OidcClientOptions;

namespace HelseIdSampleApp.Logic
{
    public interface IAuthenticationOperations
    {
        /// <summary>
        /// Initializes the Authorization code grant type and returns an access token if the authentication is successful
        /// </summary>
        string InitAuthorizationCodeGrant();

        /// <summary>
        /// Access token that can be used to access secured resources
        /// </summary>
        string AccessToken { get; } 

        /// <summary>
        /// True when the user is authenticated
        /// </summary>
        bool IsAuthenticated { get; }
    }

    public class AuthenticationOperations : IAuthenticationOperations
    {
        private readonly IConfigurations configurations;
        private readonly IBrowserFactory browserFactory;

        public string AccessToken { get; private set; }
        public bool IsAuthenticated { get; private set; }

        public AuthenticationOperations(IConfigurations configurations, IBrowserFactory browserFactory)
        {
            this.configurations = configurations;
            this.browserFactory = browserFactory;
        }

        public string InitAuthorizationCodeGrant()
        {
            var clientOptions = LoadOpenIdOptions();
            var browser = browserFactory.Browser.WebBrowser;
            clientOptions.Browser = browser;

            var oidcClient = new OidcClient(clientOptions);
            var discoClient = new DiscoveryClient(configurations.HelseIdEndpoint);
            var discoveryResponse = discoClient.GetAsync();
            var disco = discoveryResponse.GetAwaiter().GetResult();

            if (disco.IsError)
            {
                throw new ArgumentException(disco.Error);
            }

            var result = oidcClient.LoginAsync(new LoginRequest()
            {
                BackChannelExtraParameters = GetBackChannelExtraParameters(disco, clientOptions.ClientId, configurations.HelseIdCertificateThumbprint, null, JwtGenerator.SigningMethod.X509EnterpriseSecurityKey),
                // Set idporten as default provider
                //FrontChannelExtraParameters = new { acr_values = "idp:idporten-oidc Level4", prompt = "Login" }
            });

            var res = result.GetAwaiter().GetResult();

            if (res.IsError)
            {
                throw new ArgumentException(res.Error);
            }

            IsAuthenticated = true;
            return AccessToken = res?.AccessToken;
        }

        private object GetBackChannelExtraParameters(DiscoveryResponse disco, string clientId, string certificateThumbprint, string OrgEnhId, JwtGenerator.SigningMethod signingMethod)
        {
            OpenIdConnect.ClientAssertion assertion = null;

            if (signingMethod == JwtGenerator.SigningMethod.RsaSecurityKey)
            {
                assertion = OpenIdConnect.ClientAssertion.CreateWithRsaKeys(clientId, disco.TokenEndpoint, OrgEnhId);
            }

            if (signingMethod == JwtGenerator.SigningMethod.X509EnterpriseSecurityKey)
            {
                assertion = OpenIdConnect.ClientAssertion.CreateWithEnterpriseCertificate(clientId, disco.TokenEndpoint, certificateThumbprint);
            }

            return new
            {
                assertion?.client_assertion,
                assertion?.client_assertion_type,
            };
        }

        private OidcClientOptions LoadOpenIdOptions()
        {
            return new OidcClientOptions
            {
                RedirectUri = configurations.HelseIdRedirectUrl,
                ClientId = configurations.HelseIdClientId,
                Authority = configurations.HelseIdEndpoint,
                Scope = configurations.HelseIdClientConfigScope,
                ResponseMode = AuthorizeResponseMode.Redirect,
                Flow = AuthenticationFlow.AuthorizationCode,
                LoadProfile = true,
            };
        }
    }
}