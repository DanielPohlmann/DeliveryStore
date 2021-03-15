using Delivery.Clients.Service.AutoMapper;
using Delivery.Products.Service.AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Delivery.Store.Api.Configuration
{
    public static class AutoMapperConfig
    {
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(
                typeof(ViewModelToDomainMappingProduct), 
                typeof(ViewModelToDomainMappingClient),
                typeof(DomainToViewModelMappingClient)
            );
        }
    }
}
