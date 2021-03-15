using AutoMapper;
using Delivery.Core.DomainObjects;
using Delivery.Core.Notifications;
using Delivery.Core.Resources;
using Delivery.Products.Domain.Entitys;
using Delivery.Products.Domain.Interfaces;
using Delivery.Products.Service.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery.Products.Service.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly INotifier _notifier;
        private readonly IProductRepository _productRepository;
        public ProductService(INotifier notifier, IMapper mapper, IProductRepository productRepository)
        {
            _notifier = notifier;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<Product> Create(ProductCreate productModel) {
            var validateProduct = productModel.Validate();
            if (!validateProduct.IsValid) {
                _notifier.Handle(validateProduct.Errors.Select(x => new Notification(x.ErrorMessage)));
                return null;
            }

            var product = _mapper.Map<Product>(productModel);
            _productRepository.Add(product);
            if (!await _productRepository.UnitOfWork.Commit())
                _notifier.Handle(new Notification(MessagesValidation.ErroSaveData));

            return product;
        }

        public Task<Product> GetById(Guid id) => _productRepository.GetById(id);
    }
}
