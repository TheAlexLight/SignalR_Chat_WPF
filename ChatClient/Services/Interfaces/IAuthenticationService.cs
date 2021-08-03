using EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> Register(string email, string username, string password, string confirmPassword);
        Task<Account> Login(string username, string password);
    }
}
