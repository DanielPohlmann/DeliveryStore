using AutoMapper;
using Delivery.Clients.Domain.Interfaces;
using Delivery.Clients.Service.Model;
using Delivery.Clients.Service.Models;
using Delivery.Core.DomainObjects;
using Delivery.Core.Notifications;
using Delivery.Core.Resources;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery.Clients.Service.Services.Clients
{
    public class ClientService : IClientService
    {
        private readonly IMapper _mapper;
        private readonly INotifier _notifier;
        private readonly IClientRepository _clientRepository;
        public ClientService(INotifier notifier, IMapper mapper, IClientRepository clientRepository)
        {
            _notifier = notifier;
            _mapper = mapper;
            _clientRepository = clientRepository;
        }

        public async Task<Client> Create(ClientCreate clientModel) {
            var validateProduct = clientModel.Validate();
            if (!validateProduct.IsValid) {
                _notifier.Handle(validateProduct.Errors.Select(x => new Notification(x.ErrorMessage)));
                return null;
            }

            var clienteExistente = await _clientRepository.GetByCpf(clientModel.Cpf);
            if (clienteExistente != null)
            {
                _notifier.Handle(new Notification(MessagesValidation.CpfExist));
                return null;
            }

            var client = _mapper.Map<Domain.Entitys.Client>(clientModel);
            _clientRepository.Add(client);
            if (!await _clientRepository.UnitOfWork.Commit())
                _notifier.Handle(new Notification(MessagesValidation.ErroSaveData));

            return _mapper.Map<Client>(client); 
        }

        public async Task<Client> GetById(Guid id) {
            var client = await _clientRepository.GetById(id);
            return client == null ? null : _mapper.Map<Client>(client);
        } 
    }
}
