using Newtonsoft.Json;

namespace HelseIdSampleApp.OpenIdConnect.DCR.Api
{
    public class ApiResourceRequest : ApiResource
    {
        [JsonProperty("secrets")]
        public Secret[] Secrets { get; set; }
    }
}