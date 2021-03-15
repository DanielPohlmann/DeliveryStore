using Delivery.Core.Data;
using Delivery.Products.Domain.Entitys;
using Delivery.Products.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Delivery.Orders.Data.Reposiotry
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;

        public ProductRepository(ProductContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;


        public void Add(Product product)
        {
            _context.Products.Add(product);
        }

        public async Task<Product> GetById(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
