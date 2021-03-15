using Delivery.Core.DomainObjects;
using Delivery.Products.Domain.Entitys;
using Delivery.Products.Service.Model;
using System;
using System.Threading.Tasks;

namespace Delivery.Products.Service.Services.Products
{
    public interface IProductService
    {
        Task<Product> Create(ProductCreate productModel);
        Task<Product> GetById(Guid id);
    }
}
