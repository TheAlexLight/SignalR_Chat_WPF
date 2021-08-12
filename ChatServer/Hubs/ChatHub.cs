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
using System.Threading.Tasks;

namespace ChatServer.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AccountController account;

        public ChatHub(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            account = new AccountController(_userManager, _signInManager);
        }
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task SendUserBan(string username)
        {
            UserHandler user = Account.Users.FirstOrDefault(u => u.ConnectedUsername == username);

            if (user.ConnectedIds != Context.ConnectionId)
            {
                await Clients.Client(user.ConnectedIds).SendAsync("ReceiveBan", true);
            }
        }

        public async Task SendRegistration(RegistrationUserData model)
        {
            IdentityResult completedRegistration =  await account.Register(model);

            await Clients.Caller.SendAsync("ReceiveRegistrationResult", completedRegistration);
        }

        public async Task SendLogin(LoginUserData model)
        {
            SignInResult completedLogin = await account.Login(model);

            await Clients.Caller.SendAsync("ReceiveLoginResult", completedLogin);
        }

        public async Task SendUserList()
        {
            List<string> allLoginedUsers = Account.Users.Select(u => u.ConnectedUsername).Where(s => s != null).ToList();

            List<ActiveUser> activeUsers = new();

            foreach (string username in allLoginedUsers)
            {
                activeUsers.Add(new ActiveUser
                {
                    Username = username
                });
            }

            await Clients.All.SendAsync("ReceiveUserList", activeUsers);
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
