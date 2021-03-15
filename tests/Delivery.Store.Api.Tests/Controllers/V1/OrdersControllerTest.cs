using Delivery.Core.Resources;
using Delivery.Orders.Data;
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
using System.Linq;
using System.Net.Http;
using Xunit;

namespace Delivery.Store.Api.Tests.Controllers.V1
{
    public class OrdersControllerTest
    {
        public OrdersControllerTest()
        {
            SetUpOrder();
        }

        private TestServer _server;

        public HttpClient Client { get; private set; }


        [Fact(DisplayName = "Get Shipping By Zip Code Invalid Cep")]
        [Trait("V1 - Product", "Get Shipping By Zip Code")]
        public async void GetShippingByZipCodeInvalidCep()
        {
            var cep = "1111111";

            var response = await Client.GetAsync($"/api/v1/orders/shipping/{cep}");
            var badRequest = JsonConvert.DeserializeObject<ValidationProblemDetails>(response.Content.ReadAsStringAsync().Result);
            
            var message = response.Content.ReadAsStringAsync().Result;

            response.StatusCode.Should().BeEquivalentTo(400);
            badRequest.Errors.SelectMany(x => x.Value).Should().Contain(MessagesValidation.ZipCodeValid);
        }

        [Fact(DisplayName = "Get Shipping By Zip Code Valid Cep")]
        [Trait("V1 - Product", "Get Shipping By Zip Code")]
        public async void GetShippingByZipCodeValidCep()
        {
            var cep = "23799999";

            var response = await Client.GetAsync($"/api/v1/orders/shipping/{cep}");
            var shipping = JsonConvert.DeserializeObject<decimal>(response.Content.ReadAsStringAsync().Result);

            response.StatusCode.Should().BeEquivalentTo(200);
            shipping.Should().Be(10.00m);
        }

        [Fact(DisplayName = "Get Shipping By Zip Code Not Found Cep")]
        [Trait("V1 - Product", "Get Shipping By Zip Code")]
        public async void GetShippingByZipCodeNotFoundCep()
        {
            var cep = "00000000";

            var response = await Client.GetAsync($"/api/v1/orders/shipping/{cep}");

            response.StatusCode.Should().BeEquivalentTo(404);
        }

        private void SetUpOrder()
        {

            var builder = new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    var context = new OrderContext(new DbContextOptionsBuilder<OrderContext>()
                        .UseSqlServer(InitConfigurationHelper.InitConfiguration().GetConnectionString("DefaultConnection"))
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

            Client = _server.CreateClient();
        }
    }
}
