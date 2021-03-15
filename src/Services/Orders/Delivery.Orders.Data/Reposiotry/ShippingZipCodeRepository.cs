using Delivery.Core.Data;
using Delivery.Orders.Domain.Entitys;
using Delivery.Orders.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery.Orders.Data.Reposiotry
{
    public class ShippingZipCodeRepository : IShippingZipCodeRepository
    {
        private readonly OrderContext _context;

        public ShippingZipCodeRepository(OrderContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<ShippingZipCode> GetShippingByZipCode(string cep)
        {
            return await _context.ShippingZipCodes
                .AsNoTracking()
                .Where(x =>
                    x.ZipCodeBegin.Number.CompareTo(cep) <= 0 &&
                    x.ZipCodeEnd.Number.CompareTo(cep) >= 0 &&
                    x.ExpireDateStart <= DateTime.Now &&
                    x.ExpireDateEnd >= DateTime.Now
                )
                .OrderByDescending(x => x.ZipCodeBegin.Number)
                .OrderBy(x => x.ZipCodeEnd.Number)
                .FirstOrDefaultAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

    }
}
