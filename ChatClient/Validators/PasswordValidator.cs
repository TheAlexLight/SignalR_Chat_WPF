using Pather.CSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

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

            string passwordValue = (string)GetBoundValue(value);

            string inputString = ValidateOnEmptiness(passwordValue);

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
                                         + "Password should has at least one decimal digit\n"
                                         + "Password should has at least one non letter or digit symbol";

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
                    _hasNonLetterOrDigit = true;
                }

                if (_hasUpperCaseLetter && _hasLowerCaseLetter && _hasDecimalDigit && _hasNonLetterOrDigit)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        private object GetBoundValue(object value)
        {
            if (value is BindingExpression)
            {
                BindingExpression binding = (BindingExpression)value;

                object dataItem = binding.DataItem;

                string propertyName = binding.ParentBinding.Path.Path;

                IResolver resolver = new Resolver();

                return resolver.Resolve(dataItem, propertyName);
            }
            else
            {
                // ValidationStep was RawProposedValue or ConvertedProposedValue
                // The argument is already what we want!
                return value;
            }
        }
    }
}
