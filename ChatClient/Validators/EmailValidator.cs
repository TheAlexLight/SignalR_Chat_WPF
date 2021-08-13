using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ChatClient.Validators
{
    class EmailValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);

            try
            {
                new MailAddress(value.ToString());
            }
            catch
            {
                result = new ValidationResult(false, "Email is not valid");
            }

            return result;
        }
    }
}
