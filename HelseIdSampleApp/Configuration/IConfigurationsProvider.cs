using System;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;

namespace HelseIdSampleApp.Configuration
{
    public interface IConfigurationsProvider
    {
        IConfigurationRoot Configurations { get; }
    }

    public class ConfigurationsProvider : IConfigurationsProvider
    {
        private IConfigurationRoot configurations;

        public IConfigurationRoot Configurations
        {
            get
            {
                if (configurations == null)
                {
                    Init();
                }

                return configurations;
            }
        }

        private void Init()
        {
            var machineName = Environment.MachineName;
            var basePath = Application.ExecutablePath;
            basePath = basePath.Substring(0, basePath.LastIndexOf('\\'));
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            configurations = builder.Build();
        }
    }
}