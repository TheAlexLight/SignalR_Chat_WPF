using ChatServer.Models;
using Microsoft.AspNetCore.Identity;
using SharedItems.Models;
using SharedItems.Models.AuthenticationModels;
using SharedItems.Models.StatusModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Controllers
{
    public class AccountController
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationContext _dbContext;

        public AccountController(UserManager<User> userManager, ApplicationContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<IdentityResult> Register(UserRegistrationModel model)
        {
            User user = new User { Email = model.Email, UserName = model.Username };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                User createdUser = await _userManager.FindByNameAsync(model.Username);

                _dbContext.UsersStatus.Add(new UserStatusModel()
                {
                    UserId = createdUser.Id
                });

               await _dbContext.SaveChangesAsync();

                _dbContext.BansStatus.Add(new BanStatusModel()
                {
                    UserStatusModelId = createdUser.UserStatus.Id
                });

                await _dbContext.SaveChangesAsync();
            }

            return result;
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
