using Delivery.Clients.Domain.Entitys;
using Delivery.Clients.Domain.Interfaces;
using Delivery.Core.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery.Orders.Data.Reposiotry
{
    public class ClientRepository : IClientRepository
    {
        private readonly ClientContext _context;

        public ClientRepository(ClientContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Add(Client product) => _context.Clients.Add(product);

        public async Task<Client> GetById(Guid id) => await _context.Clients.FindAsync(id);

        public async Task<Client> GetByCpf(string cpf) => await _context.Clients.Where(x => x.Cpf.Number == cpf).FirstOrDefaultAsync();

        public void Dispose() => _context?.Dispose();
    }
}
