using Newtonsoft.Json;

namespace HelseIdSampleApp.OpenIdConnect.DCR.Client
{
    public class ClientClaim
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
