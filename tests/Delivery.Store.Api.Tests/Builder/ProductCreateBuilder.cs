using Bogus;
using Delivery.Products.Service.Model;
using System;

namespace Delivery.Store.Api.Tests.Builder
{
    public class ProductCreateBuilder
    {
        protected string Title;
        protected string Description;
        protected decimal Price;
        protected bool Active;
        protected string Imagem;
        protected int Amount;

        public static ProductCreateBuilder New()
        {
            var faker = new Faker();

            return new ProductCreateBuilder
            {
                Title = faker.Random.String(100, 'a', 'z'),
                Description = faker.Random.String(500, 'a', 'z'),
                Active = faker.Random.Bool(),
                Amount = faker.Random.Int(1, int.MaxValue),
                Imagem = faker.Internet.Url(),
                Price = Decimal.Round(faker.Random.Decimal(), 4)
            };
        }

        public ProductCreateBuilder WhithTitle(string title)
        {
            Title = title;
            return this;
        }

        public ProductCreateBuilder WhithDescription(string description)
        {
            Description = description;
            return this;
        }

        public ProductCreateBuilder WhithPrice(decimal price)
        {
            Price = price;
            return this;
        }

        public ProductCreateBuilder WhithActive(bool active)
        {
            Active = active;
            return this;
        }

        public ProductCreateBuilder WhithImagem(string imagem)
        {
            Imagem = imagem;
            return this;
        }

        public ProductCreateBuilder WhithAmount(int amount)
        {
            Amount = amount;
            return this;
        }

        public ProductCreate Build()
        {
            var productCreate = new ProductCreate { 
                Active = this.Active,
                Amount = this.Amount,
                Description = this.Description,
                Imagem = this.Imagem,
                Price = this.Price,
                Title = this.Title
            };
            return productCreate;
        }
    }
}
