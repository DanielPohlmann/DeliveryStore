using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Delivery.Store.Api.Configuration
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddCheck("OrderContext", new Orders.Data.Extension.SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection")))
                .AddCheck("ProductContext", new Products.Data.Extension.SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection")))
                .AddCheck("ClientContext", new Clients.Data.Extension.SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection")))
                .AddSqlServer(configuration.GetConnectionString("DefaultConnection"), name: "BancoSQL");

            return services;
        }
    }
}
