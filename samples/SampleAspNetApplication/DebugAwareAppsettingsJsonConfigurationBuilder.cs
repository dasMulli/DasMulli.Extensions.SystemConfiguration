

using DasMulli.Extensions.SystemConfiguration;

namespace SampleAspNetApplication
{
    public class DebugAwareAppsettingsJsonConfigurationBuilder : AppSettingsJsonConfigurationBuilder
    {
#if DEBUG
        public DebugAwareAppsettingsJsonConfigurationBuilder()
        : base("Development")
        {
            
        }
#else
        public DebugAwareAppsettingsJsonConfigurationBuilder()
        : base()
        {
            
        }
#endif
    }
}