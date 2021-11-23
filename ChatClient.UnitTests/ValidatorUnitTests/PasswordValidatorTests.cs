using ChatClient.Validators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.UnitTests.ValidatorUnitTests
{
    [TestFixture]
    internal class PasswordValidatorTests
    {
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("qq")] // Less than 3 symbols
        [TestCase("MoreThanFifteen.")] // More than 15 symbols
        [TestCase("qwe123!")] // check if upper symbol not exist
        [TestCase("QWE123!")] // check if lower symbol not exist
        [TestCase("qweQWE!")] // check if digital not exist
        [TestCase("qweQWE123")] // check if letterOrDigital not exist
        public void Validate_PasswordInconsistency_ReturnsFalseValidationResult(string password)
        {
            var validator = new PasswordValidator();

            var result = validator.Validate(password, CultureInfo.CurrentCulture);

            Assert.That(result.IsValid, Is.False);
        }

        [Test]
        public void Validate_AppropriatePassword_ReturnsTrueValidationResult()
        {
            var validator = new PasswordValidator();

            var result = validator.Validate("qweQWE123!", CultureInfo.CurrentCulture);

            Assert.That(result.IsValid, Is.True);
        }
    }
}
