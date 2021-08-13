using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ChatClient.Validators
{
    public abstract class ValidationBase : ValidationRule
    {
        public ValidationBase()
        {
            MinimumLength = 3;
            MaximumLength = 15;
        }

        public string ErrorMessage { get; set; }
        public int MinimumLength { get; set; }
        public int MaximumLength { get; set; }

        public string ValidateOnEmptiness(string value)
        {
            string result = string.Empty;

            if (!string.IsNullOrWhiteSpace(value))
            {
                result = value.ToString();
            }
            else
            {
                ErrorMessage = string.Format("The username can't be empty");
            }

            return result;
        }

        public bool MinimimLengthValidation(string inputString, string message)
        {
            bool result = true;
            if (inputString.Length < MinimumLength)
            {
                ErrorMessage = string.Format(message, MinimumLength);
                result = false;
            }

            return result;
        }

        public bool MaximumLengthValidation(string inputString, string message)
        {
            bool result = true;

            if (inputString.Length > MaximumLength && MaximumLength > 0)
            {
                ErrorMessage = message;
                result = false;
            }

            return result;
        }
    }
}
