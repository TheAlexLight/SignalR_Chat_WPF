using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ChatClient.Validators
{
    public class UsernameValidator : ValidationBase
    {
        private const string MINIMUM_LENGTH = "The username length should be at least {0} symbols";
        private const string MAXIMUM_LENGTH = "The username length should not be greater than {0} symbols";

        public UsernameValidator()
        {
            MinimumLength = 3;
            MaximumLength = 15;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);
            string inputString = ValidateOnEmptiness((string)value);

            if (inputString == string.Empty
                    || !MinimimLengthValidation(inputString, string.Format(MINIMUM_LENGTH, MinimumLength))
                    || !MaximumLengthValidation(inputString, string.Format(MAXIMUM_LENGTH, MaximumLength)))
            {
                result = new ValidationResult(false, ErrorMessage);
            }

            return result;
        }
    }
}
