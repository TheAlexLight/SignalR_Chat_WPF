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

        public async Task<IdentityResult> Register(UserRegistrationModel model)
        {
            User user = new User { Email = model.Email, UserName = model.Username, JoinDate = model.JoinDate };

            return await _userManager.CreateAsync(user, model.Password);
        }

        public async Task<bool> Login(UserLoginModel model)
        {
            bool result = false;

            User user = await _userManager.FindByNameAsync(model.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                result = true;
            }

            return result;
        }
    }
}
