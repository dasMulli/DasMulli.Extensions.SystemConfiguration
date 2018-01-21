using System;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using ConfigurationBuilder = System.Configuration.ConfigurationBuilder;
using ConfigurationSection = System.Configuration.ConfigurationSection;

namespace DasMulli.Extensions.SystemConfiguration
{
    public class AppSettingsJsonConfigurationBuilder : ConfigurationBuilder
    {
        private readonly IConfigurationRoot configuration;

        public AppSettingsJsonConfigurationBuilder()
            : this(AppSettingsJsonConfiguration.Default.Configuration)
        {
        }

        public AppSettingsJsonConfigurationBuilder(string environmentName)
            : this(AppSettingsJsonConfiguration.Build(environmentName).Configuration)
        {
        }

        public AppSettingsJsonConfigurationBuilder(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        public override ConfigurationSection ProcessConfigurationSection(ConfigurationSection configSection)
        {
            switch (configSection)
            {
                case AppSettingsSection appSettingsSection:
                    return ProcessAppSettingsSection(appSettingsSection);
                case ConnectionStringsSection connectionStringsSection:
                    return ProcessConnectionStringsSection(connectionStringsSection);
                default:
                    return base.ProcessConfigurationSection(configSection);
            }
        }

        private AppSettingsSection ProcessAppSettingsSection(AppSettingsSection section)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            if (appSettingsSection == null)
            {
                return section;
            }

            foreach (var setting in appSettingsSection.GetChildren())
            {
                if (setting.Value == null)
                {
                    continue;
                }

                if (section.Settings[setting.Key] is KeyValueConfigurationElement existingElement)
                {
                    existingElement.Value = setting.Value;
                }
                else
                {
                    section.Settings.Add(setting.Key, setting.Value);
                }
            }

            appSettingsSection.GetReloadToken().RegisterChangeCallback(AppSettingsSectionChanged, appSettingsSection);

            return section;
        }

        private static void AppSettingsSectionChanged(object configSectionObj)
        {
            var configSection = (IConfigurationSection)configSectionObj;
            foreach (var setting in configSection.GetChildren())
            {
                if (setting.Value == null)
                {
                    continue;
                }

                ConfigurationManager.AppSettings.Set(setting.Key, setting.Value);
            }

            configSection.GetReloadToken().RegisterChangeCallback(AppSettingsSectionChanged, configSection);
        }

        private ConnectionStringsSection ProcessConnectionStringsSection(ConnectionStringsSection section)
        {
            var connectionStringsSection = configuration.GetSection("ConnectionStrings");
            if (connectionStringsSection == null)
            {
                return section;
            }

            var connectionStringSettingsCollection = section.ConnectionStrings;

            foreach (var setting in connectionStringsSection.GetChildren())
            {
                var key = setting.Key;
                string value;
                string providerName = null;
                if (setting.Value == null)
                {
                    value = setting["ConnectionString"];
                    providerName = setting["ProviderName"];
                }
                else
                {
                    value = setting.Value;
                }

                if (value == null)
                {
                    continue;
                }

                if (connectionStringSettingsCollection[key] is ConnectionStringSettings existingConnectionString)
                {
                    existingConnectionString.ConnectionString = value;
                    if (providerName != null)
                    {
                        existingConnectionString.ProviderName = providerName;
                    }
                }
                else
                {
                    connectionStringSettingsCollection.Add(providerName != null
                        ? new ConnectionStringSettings(key, value, providerName)
                        : new ConnectionStringSettings(key, value));
                }
            }

            return section;
        }
    }
}
