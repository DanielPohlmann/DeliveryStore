using Delivery.Api.Core.Controllers;
using Delivery.Core.DomainObjects;
using Delivery.Core.Notifications;
using Delivery.Core.Resources;
using Delivery.Orders.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Delivery.Store.Api.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/orders")]
    public class OrdersController : MainController
    {
        private readonly IShippingZipCodeRepository _shippingZipCodeRepository;
        public OrdersController(
            INotifier notifier,
            IShippingZipCodeRepository shippingZipCodeRepository) : base (notifier)
        {
            _shippingZipCodeRepository = shippingZipCodeRepository;
        }

        [HttpGet("shipping/{zipcode}")]
        public async Task<ActionResult<decimal>> GetShippingByZipCode(string zipcode)
        {
            var validate = RoleValidate
                .New()
                .When(!ZipCode.Validate(zipcode), MessagesValidation.ZipCodeValid);

            if (!validate.IsValid())
                return CustomResponseError<decimal>(validate.Errors);

            var shipping = await _shippingZipCodeRepository.GetShippingByZipCode(zipcode);
            return shipping == null ? NotFound() : CustomResponseOk(shipping.Price);
        }
    }
}