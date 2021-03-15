using Delivery.Core.Resources;
using FluentValidation;
using FluentValidation.Results;
using System;

namespace Delivery.Clients.Service.Model
{
    public class ClientCreate
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public DateTime BirthDate { get; set; }

        internal ValidationResult Validate() => new ClientCreateModelValidation().Validate(this);

        public class ClientCreateModelValidation : AbstractValidator<ClientCreate>
        {
            public ClientCreateModelValidation()
            {
                RuleFor(c => c.Name)
                    .NotNull().WithMessage(string.Format(MessagesValidation.FieldRequerid, nameof(Name)))
                    .NotEmpty().WithMessage(string.Format(MessagesValidation.FieldRequerid, nameof(Name)))
                    .MaximumLength(200).WithMessage(string.Format(MessagesValidation.FieldMaximumLength, nameof(Name), 200));

                RuleFor(c => c.Cpf)
                    .Must(CpfValid).WithMessage(MessagesValidation.CPFInvalid);

                RuleFor(c => c.Email)
                    .Must(EmailValid).WithMessage(MessagesValidation.EmailInvalid);

                RuleFor(c => c.BirthDate)
                   .GreaterThanOrEqualTo(DateTime.Now.AddYears(-150)) .WithMessage(string.Format(MessagesValidation.FieldBetweenThan, nameof(BirthDate), DateTime.Now.AddYears(-150).Date, DateTime.Now.AddYears(-12).Date))
                   .LessThanOrEqualTo(DateTime.Now.AddYears(-12)).WithMessage(string.Format(MessagesValidation.FieldBetweenThan, nameof(BirthDate), DateTime.Now.AddYears(-150).Date, DateTime.Now.AddYears(-12).Date));
            }

            protected static bool CpfValid(string cpf) => Core.DomainObjects.Cpf.Validar(cpf);

            protected static bool EmailValid(string email) => Core.DomainObjects.Email.Validar(email);
        }
    }
}
