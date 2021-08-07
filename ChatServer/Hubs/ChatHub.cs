using ChatServer.Helpers;
using ChatServer.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Hubs
{
    public class ChatHub : Hub
    {
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

        public async Task SendLogin(string username)
        {
            bool existUsername = Account.Users.Exists(u=>u.ConnectedUsername == username);

            if (!existUsername)
            {
               UserHandler user = Account.Users.FirstOrDefault(a => a.ConnectedIds == Context.ConnectionId);
                user.ConnectedUsername = username;

                await SendUserList();
            }

            await Clients.Caller.SendAsync("TryLogin", existUsername);
        }

        public async Task SendUserList()
        {
            List<string> allLoginedUsers = Account.Users.Select(u => u.ConnectedUsername).Where(s => s != null).ToList();

            await Clients.All.SendAsync("ReceiveUserList", allLoginedUsers);
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

            await SendConnectionInfo(false);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
