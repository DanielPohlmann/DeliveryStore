using AutoMapper;
using Delivery.Clients.Domain.Entitys;

namespace Delivery.Clients.Service.AutoMapper
{
    public class DomainToViewModelMappingClient : Profile
    {
        public DomainToViewModelMappingClient()
        {
            CreateMap<Client, Models.Client>()
                .ForMember(destination => destination.Email, opts => opts.MapFrom(source => source.Email.Address))
                .ForMember(destination => destination.Cpf, opts => opts.MapFrom(source => source.Cpf.Number));

        }
    }
}
