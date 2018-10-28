using IdentityModel.Client;
using System.Threading.Tasks;

namespace HelseIdSampleApp.OpenIdConnect.Helpers
{
    public class OidcDiscoveryHelper
    {
        public static async Task<DiscoveryResponse> GetDiscoveryDocument(string authority)
        {
            var discoClient = new DiscoveryClient(authority);
            return await discoClient.GetAsync();
        }
    }
}
