using Newtonsoft.Json;
using System.Collections.Generic;

namespace HelseIdSampleApp.OpenIdConnect.DCR.Api
{
    public abstract class ApiResource
    {
        [JsonProperty("api_resource_id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("authorization_scopes")]
        public IEnumerable<Scope> AuthorizationScopes { get; set; }
    }
}