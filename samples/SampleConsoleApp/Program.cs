using System.Configuration;
using static System.Console;

namespace SampleConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (string settingName in ConfigurationManager.AppSettings.AllKeys)
            {
                WriteLine($"AppSetting: {settingName} = {ConfigurationManager.AppSettings[settingName]}");
            }
            foreach (ConnectionStringSettings connectionString in ConfigurationManager.ConnectionStrings)
            {
                WriteLine();
                WriteLine($"Connection string: {connectionString.Name}");
                WriteLine($"-- connection string: {connectionString.ConnectionString}");
                WriteLine($"----------- provider: {connectionString.ProviderName}");
            }
            
            ReadLine();
        }
    }
}
