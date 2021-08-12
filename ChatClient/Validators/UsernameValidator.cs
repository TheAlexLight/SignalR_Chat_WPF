using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ChatClient.Validators
{
    public class UsernameValidator : ValidationRule
    {
        public UsernameValidator()
        {
            MinimumLength = 3;
            MaximumLength = 15;
        }

        public string ErrorMessage { get; set; }
        public int MinimumLength { get; set; }
        public int MaximumLength { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);
            string inputString;

            if (!string.IsNullOrWhiteSpace((string)value))
            {
                inputString = value.ToString();
            }
            else
            {
                ErrorMessage = string.Format("The username can't be empty");
                return new ValidationResult(false, ErrorMessage);
            }

            if (inputString.Length < MinimumLength)
            {
                ErrorMessage = string.Format("The username length should be at least {0} symbols", MinimumLength);
                result = new ValidationResult(false, ErrorMessage);
            }
            else if (MaximumLength > 0 &&
                    inputString.Length > MaximumLength)
            {
                ErrorMessage = string.Format("The username length should less than {0} symbols", MaximumLength);
                result = new ValidationResult(false, ErrorMessage);
            }

            return result;
        }
    }
}
