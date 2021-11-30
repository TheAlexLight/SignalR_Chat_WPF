using ChatServer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using SharedItems.Models;
using SharedItems.Models.AuthenticationModels;
using SharedItems.Models.StatusModels;
using System;
using System.Collections.Generic;
using System.IO;
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

               await AddUserModelIds(createdUser);
               await AddUserStatuslIds(createdUser);
               await AddUserProfileIds(createdUser);
            }

            return result;
        }

        private async Task AddUserModelIds(User createdUser)
        {
            _dbContext.UserModels.Add(new UserModel()
            {
                UserId = createdUser.Id
            });

            await _dbContext.SaveChangesAsync();
        }

        private async Task AddUserStatuslIds(User createdUser)
        {
            _dbContext.UsersStatus.Add(new UserStatusModel()
            {
                UserModelId = createdUser.UserModel.Id
            });

            await _dbContext.SaveChangesAsync();

            _dbContext.BansStatus.Add(new BanStatusModel()
            {
                UserStatusModelId = createdUser.UserModel.UserStatus.Id
            });

            await _dbContext.SaveChangesAsync();

            //_dbContext.MutesStatus.Add(new MuteStatusModel()
            //{
            //    UserStatusModelId = createdUser.UserModel.UserStatus.Id
            //});

            //await _dbContext.SaveChangesAsync();
        }

        private async Task AddUserProfileIds(User createdUser)
        {
            var currentProgramPath = Environment.CurrentDirectory;
            string staticFilesFolderPath = $"{currentProgramPath}\\wwwroot"; //string.Format("{0}\\\\wwwroot}",g);//_webHostEnvironment.WebRootPath;

            string defaultImagePath = string.Format("{0}\\Images\\defaultUser.png",staticFilesFolderPath);

            _dbContext.UserProfiles.Add(new UserProfileModel()
            {
                Username = createdUser.UserName,
                Email = createdUser.Email,
                UserModelId = createdUser.UserModel.Id,
                Image = File.ReadAllBytes(defaultImagePath)
        });

            await _dbContext.SaveChangesAsync();
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
