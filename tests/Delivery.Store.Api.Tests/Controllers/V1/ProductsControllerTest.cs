using Delivery.Core.Resources;
using Delivery.Orders.Data;
using Delivery.Products.Domain.Entitys;
using Delivery.Products.Service.Model;
using Delivery.Store.Api.Tests.Builder;
using Delivery.Store.Api.Tests.Helper;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Delivery.Store.Api.Tests.Controllers.V1
{
    public class ProductsControllerTest
    {
        public ProductsControllerTest()
        {
            SetUpProduct();
        }

        private TestServer _server;

        public HttpClient Client { get; private set; }


        [Fact(DisplayName = "Add Product Invalid Data")]
        [Trait("V1 - Product", "Create Product")]
        public async void AddProductInvalidData()
        {
            var productCreate = ProductCreateBuilder.New().WhithTitle(null).Build();

            var response = await Client.PostAsync($"/api/v1/products/add", new StringContent(JsonConvert.SerializeObject(productCreate), Encoding.UTF8, "application/json"));
            var badRequest = JsonConvert.DeserializeObject<ValidationProblemDetails>(response.Content.ReadAsStringAsync().Result);

            response.StatusCode.Should().BeEquivalentTo(400);
            badRequest.Errors.SelectMany(x => x.Value).Should().Contain(string.Format(MessagesValidation.FieldRequerid, nameof(Products.Domain.Entitys.Product.Title)));
        }

        [Fact(DisplayName = "Add Product Success")]
        [Trait("V1 - Product", "Create Product")]
        public async void AddProduct()
        {
            var productCreate = ProductCreateBuilder.New().Build();

            var response = await GenerateCreateForm(productCreate);

            var product = JsonConvert.DeserializeObject<DomainObjects.Product>(response.Content.ReadAsStringAsync().Result, new JsonSerializerSettings());
            
            response.StatusCode.Should().BeEquivalentTo(201);
            product.Title.Should().Be(productCreate.Title);
            product.Description.Should().Be(productCreate.Description);
            product.Imagem.Should().Be(productCreate.Imagem);
            product.Price.Should().Be(productCreate.Price);
            product.Amount.Should().Be(productCreate.Amount);
            product.Active.Should().Be(productCreate.Active);
            product.Id.Should().NotBeEmpty();
            product.DateRegister.Should().BeAfter(DateTime.Now.AddHours(-1));
        }


        [Fact(DisplayName = "Get Product Success")]
        [Trait("V1 - Product", "Get Product")]
        public async void GetProduct()
        {
            var productCreate = ProductCreateBuilder.New().Build();
            var responseCreate = await GenerateCreateForm(productCreate);
            var productResultCreate = JsonConvert.DeserializeObject<DomainObjects.Product>(responseCreate.Content.ReadAsStringAsync().Result);
            SetUpProduct();

            var responseGet = await Client.GetAsync($"/api/v1/products/get/{productResultCreate.Id}");
            var productResultGet = JsonConvert.DeserializeObject<DomainObjects.Product>(responseGet.Content.ReadAsStringAsync().Result);

            productResultGet.Should().BeEquivalentTo(productResultCreate);
        }


        [Fact(DisplayName = "Get Product Not Exist")]
        [Trait("V1 - Product", "Get Product")]
        public async void GetProductNotExist()
        {
            var id = Guid.NewGuid();
            var response = await Client.GetAsync($"/api/v1/products/get/{id}");
            response.StatusCode.Should().BeEquivalentTo(404);
        }

        private async Task<HttpResponseMessage> GenerateCreateForm(ProductCreate productCreate)
        {
            var response = await Client.PostAsync($"/api/v1/products/add", new StringContent(JsonConvert.SerializeObject(productCreate), Encoding.UTF8, "application/json"));

            return response;
        }

        private void SetUpProduct()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    var configuration = InitConfigurationHelper.InitConfiguration();
                    var context = new ProductContext(new DbContextOptionsBuilder<ProductContext>()
                        .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                        .EnableSensitiveDataLogging()
                        .Options);

                    services.RemoveAll(typeof(OrderContext));
                    services.AddSingleton(context);
                  
                    context.Database.Migrate();
                    context.Database.OpenConnection();
                    context.Database.EnsureCreated();

                    context.SaveChanges();

                    foreach (var entity in context.ChangeTracker.Entries().ToList())
                    {
                        entity.State = EntityState.Detached;
                    }
                });

            _server = new TestServer(builder);

            Client =  _server.CreateClient();
        }
    }
}
