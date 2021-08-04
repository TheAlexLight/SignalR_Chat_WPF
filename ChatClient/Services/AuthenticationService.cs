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
        private readonly IDataService<Account> _accountService;

        public AuthenticationService(IDataService<Account> accountService)
        {
            _accountService = accountService;
        }

        public  Task<Account> Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Register(string email, string username, string password, string confirmPassword)
        {
            bool success = false;

            if (password == confirmPassword)
            {
                PasswordHasher hasher = new PasswordHasher();

               string hashedPassword =  hasher.HashPassword(password);

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
