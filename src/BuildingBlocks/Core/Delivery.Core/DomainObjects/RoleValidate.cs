using System.Collections.Generic;
using System.Linq;

namespace Delivery.Core.DomainObjects
{
    public class RoleValidate
    {
        private readonly List<string> _errors;

        private RoleValidate()
        {
            _errors = new List<string>();
        }

        public static RoleValidate New()
        {
            return new RoleValidate();
        }

        public RoleValidate When(bool temErro, string mensagemDeErro)
        {
            if (temErro)
                _errors.Add(mensagemDeErro);

            return this;
        }

        public bool IsValid() => !_errors.Any();

        public List<string> Errors => _errors;

    }
}
