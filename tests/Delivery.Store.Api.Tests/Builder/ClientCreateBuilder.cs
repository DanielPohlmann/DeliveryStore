using Bogus;
using Delivery.Clients.Service.Model;
using System;
using Bogus.Extensions.Brazil;

namespace Delivery.Store.Api.Tests.Builder
{
    public class ClientCreateBuilder
    {
        protected string Name;
        protected string Email;
        protected string Cpf;
        protected DateTime BirthDate;

        public static ClientCreateBuilder New()
        {
            var faker = new Faker();

            return new ClientCreateBuilder
            {
                Name = faker.Random.String(100, 'a', 'z'),
                Email = faker.Internet.Email(),
                Cpf = faker.Person.Cpf(false),
                BirthDate = faker.Date.Between(DateTime.Now.AddYears(-150), DateTime.Now.AddYears(-12)),
            };
        }

        public ClientCreateBuilder WhithName(string name)
        {
            Name = name;
            return this;
        }

        public ClientCreateBuilder WhithEmail(string email)
        {
            Email = email;
            return this;
        }

        public ClientCreateBuilder WhithCpf(string cpf)
        {
            Cpf = cpf;
            return this;
        }

        public ClientCreateBuilder WhithBirthDate(DateTime birthDate)
        {
            BirthDate = birthDate;
            return this;
        }

        public ClientCreate Build()
        {
            var clientCreate = new ClientCreate() {
                Name = this.Name,
                Email = this.Email,
                Cpf = this.Cpf,
                BirthDate = this.BirthDate,
            };
            return clientCreate;
        }
    }
}
