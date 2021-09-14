using ChatServer.Controllers;
using ChatServer.Helpers;
using ChatServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SharedItems.Models.AuthenticationModels;
using SharedItems.Models.StatusModels;

namespace ChatServer.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext _dbContext;
        private readonly AccountController _account;
        private readonly RoleController _roleController;

        public ChatHub(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;

            _account = new AccountController(_userManager, _dbContext);
            _roleController = new RoleController(_roleManager, _userManager);
        }

        public async Task SendRegistration(UserRegistrationModel model)
        {
            AuthorizationHelper helper = new AuthorizationHelper();
            string error = await helper.TryRegistration(model, _userManager, _account);

            bool result = false;

            if (error == "")
            {
                result = true;

                List<string> addedRoles = new List<string>()
                {
                    "User"
                };

                await _roleController.Assign(model.Username, addedRoles);
            }

            await Clients.Caller.SendAsync("ReceiveRegistrationResult", result, error);
        }

        public async Task SendLogin(UserLoginModel model)
        {
            bool successfulLogin = await _account.Login(model);

            await Clients.Caller.SendAsync("ReceiveLoginResult", successfulLogin);

            if (successfulLogin)
            {
                UserHandler userHandler = Account.Users.FirstOrDefault(a => a.ConnectedIds == Context.ConnectionId);
                userHandler.ConnectedUsername = model.Username;

                if (userHandler != null)
                {
                    User user = _dbContext.Users
                        .Include(u => u.UserStatus)
                        .ThenInclude(userStatus => userStatus.BanStatus)
                        .FirstOrDefault(u => u.UserName == model.Username);

                    await SendCurrentUser(userHandler);

                    if (user.UserStatus.BanStatus.IsBanned)
                    {
                        await Clients.Client(userHandler.ConnectedIds).SendAsync("ReceiveBan", user.UserStatus.BanStatus);
                        return;
                    }

                    #region
                    //List<IdentityError> errors = await _roleController.Create("User");
                    //List<IdentityError> errors2 = await _roleController.Create("Admin");

                    //await _roleController.Assign(model.Username, new List<string>()
                    //{
                    //    "Admin"
                    //});

                    //await _roleController.Delete("Admin");

                    //if (errors.Any())
                    //{
                    //    //TODO: Send result
                    //}
                    #endregion
                    await UpdateChat(userHandler);
                }
            }
        }

        public async Task SendReconnection(string username)
        {
            UserHandler userHandler = Account.Users.FirstOrDefault(a => a.ConnectedIds == Context.ConnectionId);
            userHandler.ConnectedUsername = username;

           await UpdateChat(userHandler);
        }

        private async Task UpdateChat(UserHandler userHandler)
        {
            await SendCurrentUser(userHandler);
            await SendUserList();
            await SendSavedMessages();
        }

        private async Task SendSavedMessages()
        {
            List<MessageModel> messages = await _dbContext.Messages.Include(m => m.UserInfo)
                               .Where(m => m.GroupName == "mainChat")
                               .ToListAsync();
           
            await Clients.All.SendAsync("ReceiveSavedMessages", messages);
        }

        public async Task SendCurrentUser(UserHandler userHandler)
        {
            User user = await _userManager.FindByNameAsync(userHandler.ConnectedUsername);

            string userRole = ((List<string>)await _userManager.GetRolesAsync(user))[0];

            UserProfileModel currentUser = new()
            {
                Username = userHandler.ConnectedUsername,
                Role = userRole
            };

            await Clients.Client(userHandler.ConnectedIds).SendAsync("ReceiveCurrentUser", currentUser);
        }

        public async Task SendUserList()
        {
            List<string> allLoginedUsers = Account.Users.Select(u => u.ConnectedUsername).Where(s => s != null).ToList();

            List<UserProfileModel> activeUsers = new();

            foreach (string username in allLoginedUsers)
            {
                activeUsers.Add(new UserProfileModel
                {
                    Username = username
                });
            }

            await Clients.All.SendAsync("ReceiveUserList", activeUsers);
        }

        public async Task SendMessage(MessageModel messageModel)
        {
            bool isFirstMessage = FirstMessageModel.CheckMessage(messageModel.UserInfo.Username);

            messageModel.IsFirstMessage = isFirstMessage;

            await _dbContext.AddAsync(messageModel);
            await _dbContext.SaveChangesAsync();

            await Clients.All.SendAsync("ReceiveMessage", messageModel);
        }

        public async Task SendUserBan(string username, BanStatusModel model)
        {
            UserHandler userHandler = Account.Users.FirstOrDefault(u => u.ConnectedUsername == username);

            User user = _dbContext.Users
                .Include(u => u.UserStatus)
                .ThenInclude(userStatus => userStatus.BanStatus)
                .FirstOrDefault(u => u.UserName == username);

            user.UserStatus.BanStatus = model;
            
            await _dbContext.SaveChangesAsync();

            if (model.IsBanned)
            {
                await Clients.Client(userHandler.ConnectedIds).SendAsync("ReceiveBan", model);
            }
            
            await UpdateChat(userHandler);
        }

        public async Task SendUserMute(string username, MuteStatusModel model)
        {
            UserHandler userHandler = Account.Users.FirstOrDefault(u => u.ConnectedUsername == username);

            User user = _dbContext.Users
                .Include(u => u.UserStatus)
                .ThenInclude(userStatus => userStatus.MuteStatus)
                .FirstOrDefault(u => u.UserName == username);

            user.UserStatus.MuteStatus = model;

            await _dbContext.SaveChangesAsync();

            if (model.IsMuted)
            {
                await Clients.Client(userHandler.ConnectedIds).SendAsync("ReceiveMute", model);
            }

            await UpdateChat(userHandler);
        }

        public async Task SendKickUser(string username)
        {
            UserHandler user = Account.Users.FirstOrDefault(u => u.ConnectedUsername == username);

            await Clients.Client(user.ConnectedIds).SendAsync("ReceiveKick");
        }

        public override Task OnConnectedAsync()
        {
            UserHandler user = new()
            {
                ConnectedIds = Context.ConnectionId
            };

            Account.Users.Add(user);

            return base.OnConnectedAsync();
        }

        public override async Task<Task> OnDisconnectedAsync(Exception exception)
        {
            UserHandler user = Account.Users.FirstOrDefault(a => a.ConnectedIds == Context.ConnectionId);
            
            Account.Users.Remove(user);

            await SendUserList();

            return base.OnDisconnectedAsync(exception);
        }
    }
}
