using Delivery.Api.Core.Controllers;
using Delivery.Core.Notifications;
using Delivery.Products.Domain.Entitys;
using Delivery.Products.Service.Model;
using Delivery.Products.Service.Services.Products;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Delivery.Store.Api.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/products")]
    public class ProductsController : MainController
    {
        private readonly IProductService _productService;
        public ProductsController(
            INotifier notifier,
            IProductService productService) : base (notifier)
        {
            _productService = productService;
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await _productService.GetById(id);
            return product == null ? NotFound() : CustomResponseOk(product);
        }

        [HttpPost("add")]
        public async Task<ActionResult<Product>> AddProduct([FromBody] ProductCreate productModel)
        {
            var product = await _productService.Create(productModel);
            return CustomResponseCreate(Url.Action(nameof(GetProduct), new { id = product?.Id}), product);
        }

    }

}
