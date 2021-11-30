using ChatServer.Models;
using Microsoft.AspNetCore.SignalR;
using SharedItems.Models;
using System.Linq;

namespace ChatServer.Helpers
{
    public class UserHelper
    {
        public UserHandler FindUserHandler(HubCallerContext hubContext, string username)
        {
            UserHandler userHandler = Account.Users.FirstOrDefault(a => a.ConnectedIds == hubContext.ConnectionId);
            userHandler.ConnectedUsername = username;

            return userHandler;
        }
    }
}
