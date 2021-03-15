using AutoMapper;
using Bogus;
using Delivery.Clients.Domain.Entitys;
using Delivery.Clients.Domain.Interfaces;
using Delivery.Clients.Service.AutoMapper;
using Delivery.Clients.Service.Services.Clients;
using Delivery.Core.Notifications;
using Delivery.Core.Resources;
using Delivery.Store.Api.Tests.Builder;
using ExpectedObjects;
using FluentAssertions;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Delivery.Store.Api.Tests.Service
{
    public class ClientServiceTest
    {
        private readonly INotifier _notifier;
        private readonly ClientService _clientService;
        private readonly Mock<IClientRepository> _clientRepository;
        private readonly Faker _faker;
        private readonly IMapper _mapper;

        public ClientServiceTest()
        {
            _clientRepository = new Mock<IClientRepository>();
            _notifier = new Notifier();
            _mapper = new MapperConfiguration(cfg => { 
                cfg.AddProfile<DomainToViewModelMappingClient>();
                cfg.AddProfile<ViewModelToDomainMappingClient>();
            }).CreateMapper();
            _clientService = new ClientService(_notifier, _mapper, _clientRepository.Object);
            _faker = new Faker();
            MessagesValidation.Culture = new System.Globalization.CultureInfo("pt");
        }

        [Fact(DisplayName = "Add Client Data Requerid Parameters is Null")]
        [Trait("Service - Client", "ClientService")]
        public async Task AddClientRequeridParameters()
        {
            //Arrange
            var clientCreate = ClientCreateBuilder
                .New()
                .WhithName(null)
                .WhithBirthDate(DateTime.MinValue)
                .WhithCpf(null)
                .WhithEmail(null)
                .Build();

            //Act
            var result = await _clientService.Create(clientCreate);

            //Assert
            Assert.Null(result);
            Assert.Contains(string.Format(MessagesValidation.FieldRequerid, nameof(Client.Name)), _notifier.GetNotification().Select(x => x.Message));
            Assert.Contains(MessagesValidation.CPFInvalid, _notifier.GetNotification().Select(x => x.Message));
            Assert.Contains(MessagesValidation.EmailInvalid, _notifier.GetNotification().Select(x => x.Message));
            Assert.Contains(string.Format(MessagesValidation.FieldBetweenThan, nameof(Client.BirthDate), DateTime.Now.AddYears(-150).Date, DateTime.Now.AddYears(-12).Date), _notifier.GetNotification().Select(x=>x.Message));
        }

        [Fact(DisplayName = "Add Client Data Requerid Parameters is Invalid")]
        [Trait("Service - Client", "ClientService")]
        public async Task AddClientInvalidParameters()
        {
            //Arrange
            var clientCreate = ClientCreateBuilder
                .New()
                .WhithName(_faker.Random.String(201))
                .WhithBirthDate(DateTime.Now)
                .WhithCpf(_faker.Random.String(10, '0', '1'))
                .WhithEmail(_faker.Random.String())
                .Build();

            //Act
            var result = await _clientService.Create(clientCreate);

            //Assert
            Assert.Null(result);
            Assert.Contains(string.Format(MessagesValidation.FieldMaximumLength, nameof(Client.Name), 200), _notifier.GetNotification().Select(x => x.Message));
            Assert.Contains(MessagesValidation.CPFInvalid, _notifier.GetNotification().Select(x => x.Message));
            Assert.Contains(MessagesValidation.EmailInvalid, _notifier.GetNotification().Select(x => x.Message));
            Assert.Contains(string.Format(MessagesValidation.FieldBetweenThan, nameof(Client.BirthDate), DateTime.Now.AddYears(-150).Date, DateTime.Now.AddYears(-12).Date), _notifier.GetNotification().Select(x => x.Message));
        }


        [Fact(DisplayName = "Add Client Exist")]
        [Trait("Service - Client", "ClientService")]
        public async Task AddClientExist()
        {
            //Arrange
            var clientCreate = ClientCreateBuilder.New().Build();
            var clientExpect = new Client(Guid.NewGuid(), clientCreate.Name, clientCreate.Email, clientCreate.Cpf, clientCreate.BirthDate);
            _clientRepository.Setup(x => x.GetByCpf(clientCreate.Cpf)).Returns(Task.FromResult(clientExpect));

            //Act
            var result = await _clientService.Create(clientCreate);

            //Assert
            Assert.Null(result);
            Assert.Contains(MessagesValidation.CpfExist, _notifier.GetNotification().Select(x => x.Message));
        }

        [Fact(DisplayName = "Add Client Not Save")]
        [Trait("Service - Client", "ClientService")]
        public async Task AddClientNotSave()
        {
            //Arrange
            var clientCreate = ClientCreateBuilder.New().Build();
            var client = new Client(Guid.NewGuid(), clientCreate.Name, clientCreate.Email, clientCreate.Cpf, clientCreate.BirthDate);
            _clientRepository.Setup(x => x.GetByCpf(clientCreate.Cpf)).Returns(Task.FromResult<Client>(null));
            _clientRepository.Setup(x => x.Add(client));
            _clientRepository.Setup(c => c.UnitOfWork.Commit()).Returns(Task.FromResult<bool>(false));

            //Act
            var result = await _clientService.Create(clientCreate);

            //Assert
            Assert.Contains(MessagesValidation.ErroSaveData, _notifier.GetNotification().Select(x => x.Message));
        }

        [Fact(DisplayName = "Add Client Save")]
        [Trait("Service - Client", "ClientService")]
        public async Task AddClientSave()
        {
            //Arrange
            var clientCreate = ClientCreateBuilder.New().Build();
            var client = new Client(Guid.NewGuid(), clientCreate.Name, clientCreate.Email, clientCreate.Cpf, clientCreate.BirthDate);
            _clientRepository.Setup(x => x.GetByCpf(clientCreate.Cpf)).Returns(Task.FromResult<Client>(null));
            _clientRepository.Setup(x => x.Add(client));
            _clientRepository.Setup(c => c.UnitOfWork.Commit()).Returns(Task.FromResult<bool>(true));

            //Act
            var result = await _clientService.Create(clientCreate);
            var clientExpect = new Delivery.Clients.Service.Models.Client()
            {
                Name = client.Name,
                BirthDate = client.BirthDate,
                Cpf = client.Cpf.Number,
                Email = client.Email.Address,
                Id = result.Id,
            };

            //Assert
            clientExpect.Should().BeEquivalentTo(result);
            Assert.Empty(_notifier.GetNotification().Select(x => x.Message));
        }
    }
}
