using Delivery.Core.DomainObjects;
using System;

namespace Delivery.Orders.Domain.Entitys
{
    public class ShippingZipCode : Entity, IAggregateRoot
    {
        // EF Relation
        public ShippingZipCode() { }

        public ShippingZipCode(
            Guid id,
            ZipCode zipCodeBegin,
            ZipCode zipCodeEnd,
            decimal price,
            DateTime expireDateStart,
            DateTime expireDateEnd,
            DateTime registerDate)
        {
            Id = id;
            ZipCodeBegin = zipCodeBegin;
            ZipCodeEnd = zipCodeEnd;
            Price = price;
            ExpireDateStart = expireDateStart;
            ExpireDateEnd = expireDateEnd;
            RegisterDate = registerDate;
        }

        public ZipCode ZipCodeBegin { get; private set; }
        public ZipCode ZipCodeEnd { get; private set; }
        public decimal Price { get; private set; }
        public DateTime ExpireDateStart { get; private set; }
        public DateTime ExpireDateEnd { get; private set; }
        public DateTime RegisterDate { get; private set; }

        public void DisableZipCodeShipping()
        {
            this.ExpireDateEnd = DateTime.Now;
        }
    }
}
