using ChatServer.Models;
using Microsoft.AspNetCore.Identity;
using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Controllers
{
    public class AccountController 
    {
        private readonly UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Register(RegistrationUserData model)
        {
            User user = new User { Email = model.Email, UserName = model.Username, JoinDate = model.JoinDate };

            return await _userManager.CreateAsync(user, model.Password);
        }

       
    }
}
