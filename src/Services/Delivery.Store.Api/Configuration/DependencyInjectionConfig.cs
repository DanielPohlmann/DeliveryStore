using Delivery.Clients.Domain.Interfaces;
using Delivery.Clients.Service.Services.Clients;
using Delivery.Core.Notifications;
using Delivery.Orders.Data;
using Delivery.Orders.Data.Reposiotry;
using Delivery.Orders.Domain.Interfaces;
using Delivery.Products.Domain.Interfaces;
using Delivery.Products.Service.Services.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Delivery.Store.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void ResolveDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<INotifier, Notifier>();

            services.AddScoped<OrderContext>();
            services.AddScoped<ProductContext>();
            services.AddScoped<ClientContext>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IClientService, ClientService>();

            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IShippingZipCodeRepository, ShippingZipCodeRepository>();
        }
    }
}
