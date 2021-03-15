using Delivery.Clients.Service.Model;
using Delivery.Clients.Service.Models;
using Delivery.Core.Resources;
using Delivery.Orders.Data;
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
    public class ClientsControllerTest
    {
        public ClientsControllerTest()
        {
            SetUpClient();
        }

        private TestServer _server;

        public HttpClient Client { get; private set; }


        [Fact(DisplayName = "Add Client Invalid Data")]
        [Trait("V1 - Client", "Create Client")]
        public async void AddClientInvalidData()
        {
            var clientCreate = ClientCreateBuilder.New().WhithName(null).Build();

            var response = await Client.PostAsync($"/api/v1/clients/add", new StringContent(JsonConvert.SerializeObject(clientCreate), Encoding.UTF8, "application/json"));
            var badRequest = JsonConvert.DeserializeObject<ValidationProblemDetails>(response.Content.ReadAsStringAsync().Result);

            response.StatusCode.Should().BeEquivalentTo(400);
            badRequest.Errors.SelectMany(x => x.Value).Should().Contain(string.Format(MessagesValidation.FieldRequerid, nameof(Clients.Domain.Entitys.Client.Name)));
        }

        [Fact(DisplayName = "Add Client Exist")]
        [Trait("V1 - Client", "Create Exist")]
        public async void AddClientExist()
        {
            var clientCreate = ClientCreateBuilder.New().Build();
            await GenerateCreateForm(clientCreate);
            SetUpClient();

            var response = await GenerateCreateForm(clientCreate);
            var badRequest = JsonConvert.DeserializeObject<ValidationProblemDetails>(response.Content.ReadAsStringAsync().Result, new JsonSerializerSettings());

            response.StatusCode.Should().BeEquivalentTo(400);
            badRequest.Errors.SelectMany(x => x.Value).Should().Contain(MessagesValidation.CpfExist);
        }

        [Fact(DisplayName = "Add Client Success")]
        [Trait("V1 - Client", "Create Client")]
        public async void AddClient()
        {
            var clientCreate = ClientCreateBuilder.New().Build();

            var response = await GenerateCreateForm(clientCreate);

            var client = JsonConvert.DeserializeObject<Client>(response.Content.ReadAsStringAsync().Result, new JsonSerializerSettings());
            
            response.StatusCode.Should().BeEquivalentTo(201);
            client.Id.Should().NotBeEmpty();
            client.BirthDate.Should().Be(clientCreate.BirthDate);
            client.Cpf.Should().Be(clientCreate.Cpf);
            client.Email.Should().Be(clientCreate.Email);
            client.Name.Should().Be(clientCreate.Name);
        }


        [Fact(DisplayName = "Get Client Success")]
        [Trait("V1 - Client", "Get Client")]
        public async void GetClient()
        {
            var clientCreate = ClientCreateBuilder.New().Build();
            var responseCreate = await GenerateCreateForm(clientCreate);
            var clientResultCreate = JsonConvert.DeserializeObject<Client>(responseCreate.Content.ReadAsStringAsync().Result);
            SetUpClient();

            var responseGet = await Client.GetAsync($"/api/v1/clients/get/{clientResultCreate.Id}");
            var clientResultGet = JsonConvert.DeserializeObject<Client>(responseGet.Content.ReadAsStringAsync().Result);

            clientResultGet.Should().BeEquivalentTo(clientResultCreate);
        }


        [Fact(DisplayName = "Get Client Not Exist")]
        [Trait("V1 - Client", "Get Client")]
        public async void GetClientNotExist()
        {
            var id = Guid.NewGuid();
            var response = await Client.GetAsync($"/api/v1/clients/get/{id}");
            response.StatusCode.Should().BeEquivalentTo(404);
        }

        private async Task<HttpResponseMessage> GenerateCreateForm(ClientCreate clientCreate)
        {
            var response = await Client.PostAsync($"/api/v1/clients/add", new StringContent(JsonConvert.SerializeObject(clientCreate), Encoding.UTF8, "application/json"));

            return response;
        }

        private void SetUpClient()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    var configuration = InitConfigurationHelper.InitConfiguration();
                    var context = new ClientContext(new DbContextOptionsBuilder<ClientContext>()
                        .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                        .EnableSensitiveDataLogging()
                        .Options);

                    services.RemoveAll(typeof(ClientContext));
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
