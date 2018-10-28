using Microsoft.Extensions.Configuration;
using System;

namespace HelseIdSampleApp.Configuration
{
    public static class ConfigurationExtentions
    {
        public static dynamic AppSettings(this IConfigurationRoot configuration, string name)
        {
            var section = configuration.GetSection(Constants.AppSettingsKey);
            if (section == null)
            {
                throw new ArgumentException($"{Constants.AppSettingsKey} section is missing from the appsettings.json file");
            }
            return section[name];
        }
    }
}