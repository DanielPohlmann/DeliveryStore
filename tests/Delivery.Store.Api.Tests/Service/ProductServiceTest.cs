using AutoMapper;
using Bogus;
using Delivery.Core.Notifications;
using Delivery.Core.Resources;
using Delivery.Products.Domain.Entitys;
using Delivery.Products.Domain.Interfaces;
using Delivery.Products.Service.AutoMapper;
using Delivery.Products.Service.Services.Products;
using Delivery.Store.Api.Tests.Builder;
using FluentAssertions;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Delivery.Store.Api.Tests.Service
{
    public class ProductServiceTest
    {
        private readonly INotifier _notifier;
        private readonly ProductService _productService;
        private readonly Mock<IProductRepository> _productRepository;
        private readonly Faker _faker;
        private readonly IMapper _mapper;

        public ProductServiceTest()
        {
            _productRepository = new Mock<IProductRepository>();
            _notifier = new Notifier();
            _mapper = new MapperConfiguration(cfg => { 
                cfg.AddProfile<ViewModelToDomainMappingProduct>();
            }).CreateMapper();
            _productService = new ProductService(_notifier, _mapper, _productRepository.Object);
            _faker = new Faker();
            MessagesValidation.Culture = new System.Globalization.CultureInfo("pt");
        }

        [Theory(DisplayName = "Add Product Data Requerid Parameters is Null or Empaty")]
        [Trait("Service - Product", "ProductService")]
        [InlineData("")]
        [InlineData(null)]
        public async Task AddProductRequeridParameters(string parameter)
        {
            //Arrange
            var produtCreate = ProductCreateBuilder
                .New()
                .WhithTitle(parameter)
                .WhithDescription(parameter)
                .Build();

            //Act
            var result = await _productService.Create(produtCreate);

            //Assert
            Assert.Null(result);
            Assert.Contains(string.Format(MessagesValidation.FieldRequerid, nameof(Product.Title)), _notifier.GetNotification().Select(x => x.Message));
            Assert.Contains(string.Format(MessagesValidation.FieldRequerid, nameof(Product.Description)), _notifier.GetNotification().Select(x => x.Message));
        }

        [Fact(DisplayName = "Add Product Data Requerid Parameters is Invalid")]
        [Trait("Service - Product", "ProductService")]
        public async Task AddProductInvalidParameters()
        {
            //Arrange
            var productCreate = ProductCreateBuilder
                .New()
                .WhithAmount(int.MinValue)
                .WhithPrice(int.MinValue)
                .WhithTitle(_faker.Random.String(101))
                .WhithDescription(_faker.Random.String(501))
                .WhithImagem(_faker.Random.String(101))
                .Build();

            //Act
            var result = await _productService.Create(productCreate);

            //Assert
            Assert.Null(result);
            Assert.Contains(string.Format(MessagesValidation.FieldMaximumLength, nameof(Product.Title), 100), _notifier.GetNotification().Select(x => x.Message));
            Assert.Contains(string.Format(MessagesValidation.FieldMaximumLength, nameof(Product.Description), 500), _notifier.GetNotification().Select(x => x.Message));
            Assert.Contains(string.Format(MessagesValidation.FieldMaximumLength, nameof(Product.Imagem), 100), _notifier.GetNotification().Select(x => x.Message));
            Assert.Contains(string.Format(string.Format(MessagesValidation.FieldGreaterThan, nameof(Product.Amount), 0)), _notifier.GetNotification().Select(x => x.Message));
            Assert.Contains(string.Format(MessagesValidation.FieldGreaterThan, nameof(Product.Price), 0), _notifier.GetNotification().Select(x => x.Message));
        }

        [Fact(DisplayName = "Add Product Not Save")]
        [Trait("Service - Product", "ProductService")]
        public async Task AddProductNotSave()
        {
            //Arrange
            var productCreate = ProductCreateBuilder.New().Build();
            var product = new Product(Guid.NewGuid(), productCreate.Title, productCreate.Description, productCreate.Price, productCreate.Active, productCreate.Imagem, productCreate.Amount);
            _productRepository.Setup(x => x.Add(product));
            _productRepository.Setup(c => c.UnitOfWork.Commit()).Returns(Task.FromResult<bool>(false));

            //Act
            var result = await _productService.Create(productCreate);

            //Assert
            Assert.Contains(MessagesValidation.ErroSaveData, _notifier.GetNotification().Select(x => x.Message));
        }

        [Fact(DisplayName = "Add Product Save")]
        [Trait("Service - Product", "ProductService")]
        public async Task AddProductSave()
        {
            //Arrange
            var productCreate = ProductCreateBuilder.New().Build();
            var product = new Product(Guid.NewGuid(), productCreate.Title, productCreate.Description, productCreate.Price, productCreate.Active, productCreate.Imagem, productCreate.Amount);
            _productRepository.Setup(x => x.Add(product));
            _productRepository.Setup(c => c.UnitOfWork.Commit()).Returns(Task.FromResult<bool>(true));

            //Act
            var result = await _productService.Create(productCreate);
            product.Id = result.Id;

            //Assert
            product.Should().BeEquivalentTo(result);
            Assert.Empty(_notifier.GetNotification().Select(x => x.Message));
        }
    }
}
