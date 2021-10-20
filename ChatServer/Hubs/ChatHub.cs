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
using SharedItems.Enums;
using Newtonsoft.Json;
using SharedItems.ViewModels;

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
                        .Include(u=>u.UserModel)
                            .ThenInclude(u => u.UserStatus)
                                .ThenInclude(userStatus => userStatus.BanStatus)
                        .Include(u=>u.UserModel)
                            .ThenInclude(um=>um.UserProfile)
                        .FirstOrDefault(u => u.UserName == model.Username);

                    user.UserModel.UserProfile.IsOnline = true;
                    await _dbContext.SaveChangesAsync();

                   // await SendCurrentUser(userHandler);

                    if (user.UserModel.UserStatus.BanStatus.IsBanned)
                    {
                        await Clients.Client(userHandler.ConnectedIds).SendAsync("ReceiveBan", user.UserModel.UserStatus.BanStatus);
                        return;
                    }

                    await SendPublicGroup();
                    #region roleAssign
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

        private async Task SendPublicGroup()
        {
            ChatGroupModel group = _dbContext.Groups
                .Include(g=>g.Users)
                .FirstOrDefault(g => g.Name == ChatType.Public);

            if (group == null)
            {
                group = new ChatGroupModel()
                {
                    Name = ChatType.Public
                };

                await _dbContext.Groups.AddAsync(group);
            }

            group.Users = _dbContext.UserModels.ToList();

            await _dbContext.SaveChangesAsync();

            string serializedGroup = JsonConvert.SerializeObject(group, Formatting.None,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            await Clients.All.SendAsync("ReceiveCurrentGroup", serializedGroup);
        }

        public async Task SendReconnection(string username)
        {
            UserHandler userHandler = Account.Users.FirstOrDefault(a => a.ConnectedIds == Context.ConnectionId);
            userHandler.ConnectedUsername = username;

           await UpdateChat(userHandler);
        }

        public async Task SendSwitchChat(ChatType chatType)
        {
            ChatGroupModel group;

            if (chatType == ChatType.Public)
            {
                group = _dbContext.Groups
                      .FirstOrDefault(g => g.Name == ChatType.Public);
            }
            else
            {
                group = new ChatGroupModel();
            }

            string serializedGroup = JsonConvert.SerializeObject(group, Formatting.None,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });


            await Clients.Caller.SendAsync("ReceiveCurrentGroup", serializedGroup);
        }

        public async Task SendUpdatePrivateMessages(UserModel selectedUser, UserModel currentUser)
        {
            ChatGroupModel group = _dbContext.Groups
                    .FirstOrDefault(g => g.Name == ChatType.Private 
                        && g.Users.Contains(selectedUser)
                        && g.Users.Contains(currentUser));

            if (group == null)
            {
                group = new ChatGroupModel()
                { 
                    Name = ChatType.Private
                };
                group.Users = new List<UserModel>();

                group.Users.Add(_dbContext.UserModels
                        .FirstOrDefault(u => u.UserProfile.Username
                            == selectedUser.UserProfile.Username));
                group.Users.Add(_dbContext.UserModels
                        .FirstOrDefault(u => u.UserProfile.Username
                            == currentUser.UserProfile.Username));
                //group.Users.Add(currentUser);

                _dbContext.Groups.Add(group);
                int result = await _dbContext.SaveChangesAsync();
            }

            string serializedGroup = JsonConvert.SerializeObject(group, Formatting.None,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            await Clients.Caller.SendAsync("ReceiveCurrentGroup", serializedGroup);
        }

        private async Task UpdateChat(UserHandler userHandler)
        {
            await SendCurrentUser(userHandler);
            await SendUserList();
            //await SendSavedMessages();
        }

        public async Task SendCurrentUser(UserHandler userHandler)
        {
            User user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.UserName == userHandler.ConnectedUsername);
            
            string userRole = ((List<string>)await _userManager.GetRolesAsync(user))[0];

            user.UserModel.UserProfile.Role = userRole;
            user.UserModel.UserProfile.Username = user.UserName;

            await _dbContext.SaveChangesAsync();

            await Clients.Client(userHandler.ConnectedIds).SendAsync("ReceiveCurrentUser", user.UserModel);
        }

        public async Task SendUserList()
        {
            List<UserModel> users = _dbContext.UserModels.ToList();

            await Clients.All.SendAsync("ReceiveUserList", users);
        }

        public async Task SendMessage(string message, ChatGroupModel currentGroup, UserModel selectedUser, UserModel currentUser)
        {
            ChatGroupModel group;
            MessageModel messageModel = JsonConvert.DeserializeObject<MessageModel>(message);

            if (currentGroup.Name == ChatType.Public)
            {
                group = _dbContext.Groups.FirstOrDefault(g => g.Name == currentGroup.Name);
            }
            else
            {
               group = _dbContext.Groups
                        .FirstOrDefault(g => g.Name == currentGroup.Name 
                        && g.Users.Contains(selectedUser)
                        && g.Users.Contains(currentUser));
            }

            if (group != null)
            {
                messageModel.IsFirstMessage = FirstMessageModel.CheckMessage(messageModel.UserModel.UserProfile.Username);
                group.Messages.Add(messageModel);

                await _dbContext.SaveChangesAsync();

                string serializedGroup = JsonConvert.SerializeObject(group, Formatting.None,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                await SendConcreteGroup(currentGroup.Name, serializedGroup);
            }
        }

        private async Task SendConcreteGroup(ChatType groupName, string group)
        {
            if (groupName == ChatType.Public)
            {
                await Clients.All.SendAsync("ReceiveCurrentGroup", group);
            }
            else
            {
             await Clients.Caller.SendAsync("ReceiveCurrentGroup", group);
            }
        }

        public async Task SendChangePhoto(UserModel currentUser, byte[] photo)
        {
            UserModel userModel = _dbContext.UserModels.FirstOrDefault(um => um == currentUser);

            userModel.UserProfile.Image = photo;

            await _dbContext.SaveChangesAsync();

            UserHandler userHandler = Account.Users.FirstOrDefault(u => u.ConnectedUsername == currentUser.UserProfile.Username);

            await UpdateChat(userHandler);

            //await Clients.Caller.SendAsync("ReceiveCurrentGroup", userModel);
        }

        public async Task SendUserBan(string username, BanStatusModel model)
        {
            UserHandler userHandler = Account.Users.FirstOrDefault(u => u.ConnectedUsername == username);

            User user = _dbContext.Users
                .Include(u => u.UserModel.UserStatus)
                .ThenInclude(userStatus => userStatus.BanStatus)
                .FirstOrDefault(u => u.UserName == username);

            user.UserModel.UserStatus.BanStatus = model;
            
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
                .Include(u => u.UserModel.UserStatus)
                .ThenInclude(userStatus => userStatus.MuteStatus)
                .FirstOrDefault(u => u.UserName == username);

            user.UserModel.UserStatus.MuteStatus = model;

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

            if (user.ConnectedUsername != null)
            {
               var dbUser = _dbContext.Users.Include(u => u.UserModel)
                    .ThenInclude(um => um.UserProfile)
                    .FirstOrDefault(u => u.UserName == user.ConnectedUsername);

                dbUser.UserModel.UserProfile.IsOnline = false;
                await _dbContext.SaveChangesAsync();
            }

            Account.Users.Remove(user);

            //await SendUserList();

            
            return base.OnDisconnectedAsync(exception);
        }
    }
}
