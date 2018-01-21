using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Reflection;

namespace DasMulli.Extensions.SystemConfiguration
{
    public class AppSettingsJsonConfiguration
    {
        private static readonly Lazy<AppSettingsJsonConfiguration> DefaultLazy =
            new Lazy<AppSettingsJsonConfiguration>(BuildDefault);

        public static AppSettingsJsonConfiguration Default => DefaultLazy.Value;

        private AppSettingsJsonConfiguration(IConfigurationRoot configuration)
        {
            Configuration = configuration;
        }

        public IConfigurationRoot Configuration { get; }
        
        public static AppSettingsJsonConfiguration BuildDefault()
        {
            var environmentName = DetermineCurrentEnvironmentName();
            return Build(environmentName);
        }

        public static AppSettingsJsonConfiguration Build(string currentEnvironmentName)
        {
            if (currentEnvironmentName == null)
            {
                currentEnvironmentName = "Production";
            }

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{currentEnvironmentName}.json", optional: true, reloadOnChange: true)
                .Build();

            return new AppSettingsJsonConfiguration(configuration);
        }

        public static string DetermineCurrentEnvironmentName()
        {
            return Environment.GetEnvironmentVariable("ASPNET_ENVIRONMENT") ??
                   Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
                   FindDefaultEnvironmentNameFromAssemblyAttributesInEntryAssembly();
        }

        private static string FindDefaultEnvironmentNameFromAssemblyAttributesInEntryAssembly() =>
            Assembly.GetEntryAssembly()?.GetCustomAttribute<DefaultEnvironmentAttribute>()?.DefaultEnvironmentName;
    }
}
