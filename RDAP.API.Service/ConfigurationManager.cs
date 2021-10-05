using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace RDAP.API.Service
{
    public class ConfigurationManager
    {
        public static IConfiguration AppSetting { get; }
        static ConfigurationManager()
        {
            var confBuilder = new ConfigurationBuilder()
                .SetBasePath($"{System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Config")
                .AddJsonFile("appsettings.json");
            AppSetting = confBuilder.Build();
        }
    }
}
