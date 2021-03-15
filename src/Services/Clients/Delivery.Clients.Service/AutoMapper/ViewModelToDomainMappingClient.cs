using AutoMapper;
using Delivery.Clients.Domain.Entitys;
using Delivery.Clients.Service.Model;
using Delivery.Core.DomainObjects;
using System;

namespace Delivery.Clients.Service.AutoMapper
{
    public class ViewModelToDomainMappingClient : Profile
    {
        public ViewModelToDomainMappingClient()
        {
            CreateMap<ClientCreate, Client>()
                .ConstructUsing(c => new Client(
                    Guid.NewGuid(),
                    c.Name,
                    c.Email,
                    c.Cpf,
                    c.BirthDate)
                );

            CreateMap<string, Email>()
               .ConstructUsing(c => new Email(c)
            );

            CreateMap<string, Cpf>()
              .ConstructUsing(c => new Cpf(c)
           );
        }
    }
}
