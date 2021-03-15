using Delivery.Core.DomainObjects;
using System;

namespace Delivery.Products.Domain.Entitys
{
    public class Product : Entity, IAggregateRoot
    {
        // EF Relation
        public Product() { }

        public Product(
            Guid  id,
            string title,
            string description,
            decimal price,
            bool active,
            String imagem,
            int amount)
        {
            Id = id;
            Title = title;
            Description = description;
            Price = price;
            Active = active;
            DateRegister = DateTime.Now;
            Imagem = imagem;
            Amount = amount;
        }

        public Product(
           Guid id,
           string title,
           string description,
           decimal price,
           bool active,
           String imagem,
           int amount,
           DateTime dateRegister) : this(id, title, description, price, active, imagem, amount)
        {
            DateRegister = dateRegister;
        } 

        public string Title { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public bool Active { get; private set; }
        public DateTime DateRegister { get; private set; }
        public string Imagem { get; private set; }
        public int Amount { get; private set; }
    }
}
