using AutoMapper;
using Delivery.Products.Domain.Entitys;
using Delivery.Products.Service.Model;
using System;

namespace Delivery.Products.Service.AutoMapper
{
    public class ViewModelToDomainMappingProduct : Profile
    {
        public ViewModelToDomainMappingProduct()
        {
            CreateMap<ProductCreate, Product>()
                .ConstructUsing(c => new Product(
                    Guid.NewGuid(), 
                    c.Title, 
                    c.Description, 
                    c.Price, 
                    c.Active,  
                    c.Imagem, 
                    c.Amount)
                );
          
        }
    }
}
