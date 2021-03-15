using Delivery.Core.Data;
using Delivery.Orders.Domain.Entitys;
using System.Threading.Tasks;

namespace Delivery.Orders.Domain.Interfaces
{
    public interface IShippingZipCodeRepository : IRepository<ShippingZipCode>
    {
        Task<ShippingZipCode> GetShippingByZipCode(string cep);
    }
}
