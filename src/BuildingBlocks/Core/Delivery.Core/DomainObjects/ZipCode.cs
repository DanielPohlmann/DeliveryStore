using Delivery.Core.Resources;
using System.Linq;
using System.Text.RegularExpressions;

namespace Delivery.Core.DomainObjects
{
    public class ZipCode 
    {
        public const int ZipCodeLength = 8;

        public string Number { get; private set; }

        protected ZipCode() { }

        public ZipCode(string number)
        {
            number = string.Concat(number?.Where(char.IsDigit));

            if (!Validate(number))
                throw new DomainException(MessagesValidation.ZipCodeValid);

            Number = number.ToUpper();
        }
        public static bool Validate(string number)
        {
            if (string.IsNullOrEmpty(number))
                return false;

            var regexVin = new Regex(@"^([0-9]{8})*$");
            return regexVin.IsMatch(number);
        }
    }
}
