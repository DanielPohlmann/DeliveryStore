using Delivery.Clients.Service.Model;
using Delivery.Clients.Service.Models;
using System;
using System.Threading.Tasks;

namespace Delivery.Clients.Service.Services.Clients
{
    public interface IClientService
    {
        Task<Client> Create(ClientCreate productModel);
        Task<Client> GetById(Guid id);
    }
}
