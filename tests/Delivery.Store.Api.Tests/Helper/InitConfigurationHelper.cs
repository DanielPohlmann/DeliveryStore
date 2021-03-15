using Microsoft.Extensions.Configuration;

namespace Delivery.Store.Api.Tests.Helper
{
    public static class InitConfigurationHelper
    {
        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            return config;
        }
    }
}
