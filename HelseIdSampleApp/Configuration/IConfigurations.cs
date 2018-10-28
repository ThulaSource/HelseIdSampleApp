
namespace HelseIdSampleApp.Configuration
{
    public interface IConfigurations
    {
        /// <summary>
        /// The HelseId enabler client identifier
        /// </summary>
        string HelseIdClientId { get; }

        /// <summary>
        /// The HelseId enabler configured redirect Url
        /// </summary>
        string HelseIdRedirectUrl { get; }

        /// <summary>
        /// The HelseId enabler certificate thumbprint
        /// </summary>
        string HelseIdCertificateThumbprint { get; }

        /// <summary>
        /// The HelseId enabler organization Enh-Id
        /// </summary>
        string HelseIdOrganizationId { get; }

        /// <summary>
        /// The HelseId enabler organization name
        /// </summary>
        string HelseIdOrganizationName { get; }

        /// <summary>
        /// The HelseId scope used to fetch a token to register a new client
        /// </summary>
        string HelseIdClientConfigScope { get; }

        /// <summary>
        /// The HelseId new client scope 
        /// </summary>
        string HelseIdScope { get; }

        /// <summary>
        /// The HelseId login endpoint
        /// </summary>
        string HelseIdEndpoint { get; }

        /// <summary>
        /// The HelseId API endpoint
        /// </summary>
        string HelseIdApiEndpoint { get; }
    }

    public class Configurations : IConfigurations
    {
        private readonly IConfigurationsProvider configProvider;

        public Configurations(IConfigurationsProvider configProvider)
        {
            this.configProvider = configProvider;
        }

        public string HelseIdClientId => configProvider.Configurations.AppSettings(Constants.HelseIdClientIdKey);
        public string HelseIdRedirectUrl => configProvider.Configurations.AppSettings(Constants.HelseIdRedirectUrlKey);
        public string HelseIdCertificateThumbprint => configProvider.Configurations.AppSettings(Constants.HelseIdCertificateThumbprintKey);
        public string HelseIdOrganizationId => configProvider.Configurations.AppSettings(Constants.HelseIdOrgIdKey);
        public string HelseIdOrganizationName => configProvider.Configurations.AppSettings(Constants.HelseIdOrgNameKey);
        public string HelseIdClientConfigScope => configProvider.Configurations.AppSettings(Constants.HelseIdClientConfigScopeKey);
        public string HelseIdScope => configProvider.Configurations.AppSettings(Constants.HelseIdScopeKey);
        public string HelseIdEndpoint => configProvider.Configurations.AppSettings(Constants.HelseIdEndpointKey);
        public string HelseIdApiEndpoint => configProvider.Configurations.AppSettings(Constants.HelseIdApiEndpointKey);
        
    }
}