using Delivery.Order.Domain;
using Delivery.Order.Domain.DTOs;
using System.Threading.Tasks;

namespace Delivery.Order.Service.ShippingZipCodes
{
    public interface IShippingZipCodeService
    {
        Task<ShippingZipCode> GetShippingByZipCode(ShippingZipCodeModel cep);
    }
}
