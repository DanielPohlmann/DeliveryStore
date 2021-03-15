using System;

namespace Delivery.Store.Api.Tests.DomainObjects
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Title { get;  set; }
        public string Description { get;  set; }
        public decimal Price { get;  set; }
        public bool Active { get;  set; }
        public DateTime DateRegister { get;  set; }
        public string Imagem { get;  set; }
        public int Amount { get;  set; }
    }
}
