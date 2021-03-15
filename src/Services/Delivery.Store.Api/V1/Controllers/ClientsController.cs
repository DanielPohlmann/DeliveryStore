using Delivery.Api.Core.Controllers;
using Delivery.Clients.Service.Model;
using Delivery.Clients.Service.Models;
using Delivery.Clients.Service.Services.Clients;
using Delivery.Core.Notifications;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Delivery.Store.Api.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/clients")]
    public class ClientsController : MainController
    {
        private readonly IClientService _clientService;
        public ClientsController(
            INotifier notifier,
            IClientService productService) : base (notifier)
        {
            _clientService = productService;
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<Client>> GetClient(Guid id)
        {
            var client = await _clientService.GetById(id);
            return client == null ? NotFound() : CustomResponseOk(client);
        }

        [HttpPost("add")]
        public async Task<ActionResult<Client>> AddClient([FromBody] ClientCreate clientModel)
        {
            var client = await _clientService.Create(clientModel);
            return CustomResponseCreate(Url.Action(nameof(GetClient), new { id = client?.Id}), client);
        }

    }

}
