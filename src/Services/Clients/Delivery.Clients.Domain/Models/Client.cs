using Delivery.Core.DomainObjects;
using System;

namespace Delivery.Clients.Domain.Entitys
{
    public class Client : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public Email Email { get; private set; }
        public Cpf Cpf { get; private set; }
        public DateTime BirthDate { get; private set; }
        public bool Excluded { get; private set; }
        

        // EF Relation
        protected Client() { }

        public Client(Guid id, string name, string email, string cpf, DateTime birthDate)
        {
            Id = id;
            Name = name;
            BirthDate = birthDate;
            Email = new Email(email);
            Cpf = new Cpf(cpf);
            Excluded = false;
        }

    }
}
