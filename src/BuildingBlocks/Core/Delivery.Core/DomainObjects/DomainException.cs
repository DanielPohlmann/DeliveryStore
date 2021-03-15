using System;
using System.Collections.Generic;

namespace Delivery.Core.DomainObjects
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
        public DomainException(List<string> mensagensDeErros) : base(string.Join("; ", mensagensDeErros)) { }
       
    }
}
