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
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> Register(RegistrationUserData model)
        {
            User user = new User { Email = model.Email, UserName = model.Username, JoinDate = model.JoinDate };

            return await _userManager.CreateAsync(user, model.Password);
        }

        public async Task<SignInResult> Login(LoginUserData model)
        {
            return await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
        }

        public async Task Logout()
        {
           await _signInManager.SignOutAsync();
        }
    }
}
