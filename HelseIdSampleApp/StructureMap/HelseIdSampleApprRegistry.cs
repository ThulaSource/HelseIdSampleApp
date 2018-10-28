using HelseIdSampleApp.BrowserLogic;
using HelseIdSampleApp.Configuration;
using HelseIdSampleApp.Logic;
using StructureMap.Configuration.DSL;

namespace HelseIdSampleApp.StructureMap
{
    public class HelseIdSampleAppRegistry : Registry
    {
        public HelseIdSampleAppRegistry()
        {
            For<IBrowserFactory>().Singleton().Use<BrowserFactory>();
            For<IAuthenticationOperations>().Singleton().Use<AuthenticationOperations>();
            For<IConfigurationsProvider>().Singleton().Use<ConfigurationsProvider>();
            For<IConfigurations>().Use<Configurations>();
        }
    }
}