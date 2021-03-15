using Delivery.Order.Domain;
using Delivery.Order.Domain.DTOs;
using Delivery.Order.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Delivery.Order.Service.ShippingZipCodes
{
    public class ShippingZipCodeService : IShippingZipCodeService
    {
        private readonly IShippingZipCodeRepository _shippingZipCodeRepository;
        public ShippingZipCodeService(IShippingZipCodeRepository shippingZipCodeRepository) {
            _shippingZipCodeRepository = shippingZipCodeRepository;
        }

        public async Task<ShippingZipCode> GetShippingByZipCode(ShippingZipCodeModel model)
        {
            if (!model.IsValid().IsValid)
                throw new Exception("");

            return await _shippingZipCodeRepository.GetShippingByZipCode(model.ZipCodeNumber);
        }
    }


}
