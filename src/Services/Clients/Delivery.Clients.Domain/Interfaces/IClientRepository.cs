using Delivery.Core.Data;
using Delivery.Clients.Domain.Entitys;
using System;
using System.Threading.Tasks;

namespace Delivery.Clients.Domain.Interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        void Add(Client client);
        Task<Client> GetById(Guid id);
        Task<Client> GetByCpf(string cpf);
    }
}
