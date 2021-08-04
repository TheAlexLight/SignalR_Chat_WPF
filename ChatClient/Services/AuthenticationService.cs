using ChatClient.Encryption;
using ChatClient.Services.Interfaces;
using EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Services
{
    class AuthenticationService : IAuthenticationService
    {
        private readonly IAccountService<Account> _accountService;
        private readonly PasswordHasher _passwordHasher;

        public AuthenticationService(IAccountService<Account> accountService, PasswordHasher passwordHasher)
        {
            _accountService = accountService;
            _passwordHasher = passwordHasher;
        }

        public async Task<Account> Login(string username, string password)
        {
            Account account = await _accountService.GetByUsername(username);

            bool passwordMatch = _passwordHasher.VerifyHashedPassword(account.AccountHolder.PasswordHash, password);

            if (!passwordMatch)
            {
                throw new Exception();
            }

            return account;
        }

        public async Task<bool> Register(string email, string username, string password, string confirmPassword)
        {
            bool success = false;

            if (password == confirmPassword)
            {
               string hashedPassword =  _passwordHasher.HashPassword(password);

                User user = new User()
                {
                    Email = email,
                    Username = username,
                    PasswordHash = hashedPassword,
                    DateJoined = DateTime.Now
                };

                Account account = new Account()
                {
                    AccountHolder = user
                };

               await _accountService.Create(account);
            }

            return success;
        }

    }
}
