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
    public class EmailValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);

            if (value is string emailString && emailString != string.Empty)
            {
                if (!MailAddress.TryCreate(emailString, out MailAddress mailAddress))
                {
                    return new ValidationResult(false, "Email is not valid"); ;
                }

                // And if you want to be more strict:
                string[] hostParts = mailAddress.Host.Split('.');

                if (hostParts.Length == 1 // No dot.
                        || hostParts.Any(p => p == string.Empty) // Double dot.
                        || hostParts[^1].Length < 2 // TLD only one letter.
                        || mailAddress.User.Contains(' ')
                        || mailAddress.User.Split('.').Any(p => p == string.Empty)) // Double dot or dot at end of user part.
                {
                    result = new ValidationResult(false, "Email is not valid");
                }
            }
            else
            {
                result = new ValidationResult(false, "Email can't be empty");
            }

            return result;
        }
    }
}
