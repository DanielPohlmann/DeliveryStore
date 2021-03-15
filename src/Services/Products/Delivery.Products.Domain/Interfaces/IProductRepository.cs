using Delivery.Core.Data;
using Delivery.Products.Domain.Entitys;
using System;
using System.Threading.Tasks;

namespace Delivery.Products.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        void Add(Product product);
        Task<Product> GetById(Guid id);
    }
}
