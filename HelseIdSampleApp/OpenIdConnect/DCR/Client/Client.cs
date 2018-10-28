using Newtonsoft.Json;
using System.Collections.Generic;

namespace HelseIdSampleApp.OpenIdConnect.DCR.Client
{
    public class Client
    {
        #region OIDC

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_name")]
        public string ClientName { get; set; }

        [JsonProperty("redirect_uris")]
        public IEnumerable<string> RedirectUris { get; set; }

        [JsonProperty("grant_types")]
        public IEnumerable<string> GrantTypes { get; set; }

        #endregion OIDC

        #region IdentityServer

        [JsonProperty("absolute_refresh_token_lifetime")]
        public int? AbsoluteRefreshTokenLifetime { get; set; }

        [JsonProperty("access_token_lifetime")]
        public int? AccessTokenLifetime { get; set; }

        [JsonProperty("access_token_type")]
        public string AccessTokenType { get; set; }

        [JsonProperty("allow_access_tokens_via_browser")]
        public bool AllowAccessTokensViaBrowser { get; set; }

        [JsonProperty("allow_refresh_token")]
        public bool AllowOfflineAccess { get; set; }

        [JsonProperty("always_include_user_claims_in_identity_token")]
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }

        [JsonProperty("always_send_client_claims")]
        public bool AlwaysSendClientClaims { get; set; }

        [JsonProperty("authorization_code_lifetime")]
        public int? AuthorizationCodeLifetime { get; set; }

        [JsonProperty("identity_provider_restrictions")]
        public IEnumerable<string> IdentityProviderRestrictions { get; set; }

        [JsonProperty("identity_token_lifetime")]
        public int? IdentityTokenLifetime { get; set; }

        [JsonProperty("logout_uri")]
        public string LogoutUri { get; set; }

        [JsonProperty("post_logout_redirect_uris")]
        public IEnumerable<string> PostLogoutRedirectUris { get; set; }

        [JsonProperty("refresh_token_expiration")]
        public string RefreshTokenExpiration { get; set; }

        [JsonProperty("refresh_token_usage")]
        public string RefreshTokenUsage { get; set; }

        [JsonProperty("require_client_secret")]
        public bool RequireClientSecret { get; set; }

        [JsonProperty("sliding_refresh_token_lifetime")]
        public int? SlidingRefreshTokenLifetime { get; set; }

        [JsonProperty("client_claims")]
        public IEnumerable<ClientClaim> ClientClaims { get; set; }

        [JsonProperty("allowed_scopes")]
        public IEnumerable<string> AllowedScopes { get; set; }

        #endregion IdentityServer

        #region HelseID 

        [JsonProperty("on_behalf_of")]
        public string OnBehalfOf { get; set; }

        #endregion HelseID

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
