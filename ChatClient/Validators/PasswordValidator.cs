using ChatClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ChatClient.Validators
{
    public class PasswordValidator : ValidationBase
    {
        public PasswordValidator()
        {
            MinimumLength = 6;
            MaximumLength = 15;
        }
        private const string MINIMUM_LENGTH = "The password length should be at least {0} symbols";
        private const string MAXIMUM_LENGTH = "The password length should not be greater than {0} symbols";

        private bool _hasLowerCaseLetter;
        private bool _hasUpperCaseLetter;
        private bool _hasDecimalDigit;
        private bool _hasNonLetterOrDigit;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);
            string inputString = ValidateOnEmptiness((string)value);

            UpdateFields();

            if (inputString == string.Empty
                    || !MinimimLengthValidation(inputString, string.Format(MINIMUM_LENGTH, MinimumLength))
                    || !MaximumLengthValidation(inputString, string.Format(MAXIMUM_LENGTH, MaximumLength))
                    || !SymbolsValidation(inputString))
            {
                result = new ValidationResult(false, ErrorMessage);
            }

            return result;
        }

        private void UpdateFields()
        {
         _hasLowerCaseLetter = false;
         _hasUpperCaseLetter = false;
         _hasDecimalDigit = false;
         _hasNonLetterOrDigit = false;
    }

        private bool SymbolsValidation(string inputString)
        {
            bool result = false;
            ErrorMessage = "Password should has at least one upper letter\n" 
                                         + "Password should has at least one lower letter\n"
                                         + "Password should has at least one decimal digit"
                                         + "Password should has at least one nonLetterOrDigit symbol";

            foreach (char ch in inputString)
            {
                if (char.IsUpper(ch))
                {
                    _hasUpperCaseLetter = true;
                }
                else if (char.IsLower(ch))
                {
                    _hasLowerCaseLetter = true;
                }
                else if (char.IsDigit(ch)) 
                {
                    _hasDecimalDigit = true;
                }
                else if (!char.IsLetterOrDigit(ch))
                {
                    _hasNonLetterOrDigit = true; ;
                }

                if (_hasUpperCaseLetter && _hasLowerCaseLetter && _hasDecimalDigit && _hasNonLetterOrDigit)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
}
