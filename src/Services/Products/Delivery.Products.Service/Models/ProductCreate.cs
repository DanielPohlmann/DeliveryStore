using Delivery.Core.Resources;
using FluentValidation;
using FluentValidation.Results;
using System;

namespace Delivery.Products.Service.Model
{
    public class ProductCreate
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Active { get; set; }
        public string Imagem { get; set; }
        public int Amount { get; set; }

        internal ValidationResult Validate() => new ProductCreateModelValidation().Validate(this);

        public class ProductCreateModelValidation : AbstractValidator<ProductCreate>
        {
            public ProductCreateModelValidation()
            {

                RuleFor(c => c.Title)
                    .NotNull().WithMessage(string.Format(MessagesValidation.FieldRequerid, nameof(Title)))
                    .NotEmpty().WithMessage(string.Format(MessagesValidation.FieldRequerid, nameof(Title)))
                    .MaximumLength(100).WithMessage(string.Format(MessagesValidation.FieldMaximumLength, nameof(Title), 100));

                RuleFor(c => c.Description)
                    .NotNull().WithMessage(string.Format(MessagesValidation.FieldRequerid, nameof(Description)))
                    .NotEmpty().WithMessage(string.Format(MessagesValidation.FieldRequerid, nameof(Description)))
                    .MaximumLength(500).WithMessage(string.Format(MessagesValidation.FieldMaximumLength, nameof(Description), 500));

                RuleFor(c => c.Imagem)
                   .MaximumLength(100).WithMessage(string.Format(MessagesValidation.FieldMaximumLength, nameof(Imagem), 100));

                RuleFor(c => c.Amount)
                   .GreaterThan(0).WithMessage(string.Format(MessagesValidation.FieldGreaterThan, nameof(Amount), 0));

                RuleFor(c => c.Price)
                    .GreaterThan(0).WithMessage(string.Format(MessagesValidation.FieldGreaterThan, nameof(Price), 0));

            }
        }
    }
}
