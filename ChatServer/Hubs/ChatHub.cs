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

namespace ChatServer.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<User> _userManager;
        private readonly AccountController _account;
        private readonly ApplicationContext _dbContext;

        public ChatHub(UserManager<User> userManager, ApplicationContext dbContext)
        {
            _userManager = userManager;
            _account = new AccountController(_userManager);

            _dbContext = dbContext;
        }

        public async Task SendRegistration(RegistrationUserData model)
        {
            AuthorizationHelper helper = new AuthorizationHelper();
            string error = await helper.TryRegistration(model, _userManager, _account);

            bool result = false;

            if (error == "")
            {
                result = true;
            }

            await Clients.Caller.SendAsync("ReceiveRegistrationResult", result, error);

            //if (result)
            //{
            //    UserHandler user = Account.Users.FirstOrDefault(a => a.ConnectedIds == Context.ConnectionId);
            //    user.ConnectedUsername = model.Username;

            //    await SendCurrentUser();
            //    await SendUserList();
            //}
        }

        public async Task SendLogin(UserLoginModel model)
        {
            bool successfulLogin = await _account.Login(model);

            await Clients.Caller.SendAsync("ReceiveLoginResult", successfulLogin);

            if (successfulLogin)
            {
                UserHandler user = Account.Users.FirstOrDefault(a => a.ConnectedIds == Context.ConnectionId);
                user.ConnectedUsername = model.Username;

                await SendCurrentUser();
                await SendUserList();
                await SendSavedMessages();   
            }
        }

        private async Task SendSavedMessages()
        {
            List<MessageModel> messages = await _dbContext.Messages.Include(m => m.UserInfo)
                               .Where(m => m.GroupName == "mainChat")
                               .ToListAsync();
           //UserProfileModel a = _dbContext.Messages.Select(m => m.UserInfo).First();
            //List<MessageModel> messages = _dbContext.Messages.Select(m => m).Where(message => message.GroupName == "mainChat").ToList();

            await Clients.All.SendAsync("ReceiveSavedMessages", messages);
        }

        public async Task SendCurrentUser()
        {
            var user = Account.Users.FirstOrDefault(u => u.ConnectedIds == Context.ConnectionId);

            UserProfileModel currentUser = new()
            {
                Username = user.ConnectedUsername
            };

            await Clients.Caller.SendAsync("ReceiveCurrentUser", currentUser);
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

        public async Task SendUserBan(string username)
        {
            UserHandler user = Account.Users.FirstOrDefault(u => u.ConnectedUsername == username);

            if (user.ConnectedIds != Context.ConnectionId)
            {
                await Clients.Client(user.ConnectedIds).SendAsync("ReceiveBan", true);
            }
        }

        public async Task SendConnectionInfo(bool connected)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveConnectionInfo", connected);
        }

        public override async Task<Task> OnConnectedAsync()
        {
            UserHandler user = new()
            {
                ConnectedIds = Context.ConnectionId
            };

            Account.Users.Add(user);

            await SendConnectionInfo(true);

            return base.OnConnectedAsync();
        }

        public override async Task<Task> OnDisconnectedAsync(Exception exception)
        {
            UserHandler user = Account.Users.FirstOrDefault(a => a.ConnectedIds == Context.ConnectionId);

            Account.Users.Remove(user);

            await SendUserList();

            await SendConnectionInfo(false);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
