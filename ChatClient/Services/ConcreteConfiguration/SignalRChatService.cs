using ChatClient.Enums;
using ChatClient.Interfaces;
using MessagePack;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SharedItems.Enums;
using SharedItems.Models;
using SharedItems.Models.AuthenticationModels;
using SharedItems.Models.StatusModels;
using SharedItems.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace ChatClient.Services
{
    public class SignalRChatService : ISignalRChatService
    {
        public HubConnection Connection { get; }

        public event Action<bool, string> ReceiveRegistrationResult;
        public event Action<bool> ReceiveLoginResult;
        public event Action<UserModel> CurrentUserReceived;
        public event Action<ObservableCollection<UserModel>> UserListReceived;
        public event Action<ChatGroupModel> CurrentGroupReceived;
        //public event Action<MessageModel> MessageReceived;
        public event Action ReceivedKick;
        public event Action<BanStatusModel> ReceivedBan;
        public event Action<MuteStatusModel> ReceivedMute;
        public event Func<bool, string, string, Task> ReceivedUserPropertyChange;

        public bool IsEventHandlerRegistered()
        {
            if (ReceiveRegistrationResult != null)
            {
                return true;
                //foreach (Delegate existingHandler in ReceiveRegistrationResult.GetInvocationList())
                //{
                //    if (existingHandler == prospectiveHandler)
                //    {
                //        return true;
                //    }
                //}
            }

            return false;
        }

        public SignalRChatService(HubConnectionBuilder connectionBuilder)
        {
            Connection = connectionBuilder
                //.AddMessagePackProtocol()
                //.AddMessagePackProtocol(options =>
                //{
                //    options.SerializerOptions = MessagePackSerializerOptions.Standard
                //        .WithResolver(MessagePack.Resolvers.StandardResolver.Instance)
                //        .WithSecurity(MessagePackSecurity.UntrustedData);
                //})
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions.MaxDepth = 64;
                    options.PayloadSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                })
                    .WithUrl("http://localhost:5000/chat")
                    .WithAutomaticReconnect()
                    .Build();

            GetChatHubMessagesInvoke();
        }

        private void GetChatHubMessagesInvoke()
        {
            Connection.On<bool, string>("ReceiveRegistrationResult", (result, error) => ReceiveRegistrationResult?.Invoke(result, error));
            Connection.On<bool>("ReceiveLoginResult", (result) => ReceiveLoginResult?.Invoke(result));
            Connection.On<ObservableCollection<UserModel>>("ReceiveUserList", (users) => UserListReceived?.Invoke(users));
            Connection.On<ChatGroupModel>("ReceiveCurrentGroup", (group) => CurrentGroupReceived?.Invoke(group));
            Connection.On<UserModel>("ReceiveCurrentUser", (currentUser) => CurrentUserReceived?.Invoke(currentUser));
            //Connection.On<MessageModel>("ReceiveMessage", (messageModel) => MessageReceived?.Invoke(messageModel));
            Connection.On<BanStatusModel>("ReceiveBan", (model) => ReceivedBan?.Invoke(model));
            Connection.On<MuteStatusModel>("ReceiveMute", (model) => ReceivedMute?.Invoke(model));
            Connection.On("ReceiveKick", () => ReceivedKick?.Invoke());
            Connection.On<bool, string, string>("ReceiveUserPropertyChange", (success, propertyName, message) => ReceivedUserPropertyChange?.Invoke(success, propertyName, message));
        }

        public async Task Connect()
        {
            await Connection.StartAsync();
        }

        public async Task Reconnect(string username)
        {
            await Connection.SendAsync("SendReconnection", username);
        }

        public async Task SendMessage(MessageModel message, ChatGroupModel currentGroup, UserModel selectedUser, UserModel currentUser)
        {
            await Connection.SendAsync("SendMessage", message, currentGroup, selectedUser, currentUser);
        }

        public async Task UpdateMessage(MessageModel message, ChatGroupModel currentGroup)
        {
            await Connection.SendAsync("SendUpdateMessage", message, currentGroup);
        }

        public async Task Registration(UserRegistrationModel model)
        {
            await Connection.SendAsync("SendRegistration", model);
        }

        public async Task Login(UserLoginModel model)
        {
            await Connection.SendAsync("SendLogin", model);
        }

        public async Task SendBan(string username, BanStatusModel model)
        {
            await Connection.SendAsync("SendUserBan", username, model);
        }

        public async Task SendMute(string username, MuteStatusModel model)
        {
            await Connection.SendAsync("SendUserMute", username, model);
        }

        public async Task KickUser(string username)
        {
            await Connection.SendAsync("SendKickUser", username);
        }
        
        public async Task SwitchChat(ChatType chatTtype, UserModel currentUser)
        {
            await Connection.SendAsync("SendSwitchChat", chatTtype, currentUser);
        }
        public async Task UpdatePrivateMessages(UserModel selectedUser, UserModel currentUser)
        {
            await Connection.SendAsync("SendUpdatePrivateMessages", selectedUser, currentUser);
        }

        public async Task UpdateMessage(MessageModel message)
        {
            await Connection.SendAsync("UpdateMessage", message);
        }

        public async Task ChangePhoto(UserModel currentUser, byte[] photo)
        {
            await Connection.SendAsync("SendChangePhoto", currentUser, photo);
        }

        public async Task SubmitUsernameChange(UserModel user, string username, string password)
        {
            await Connection.SendAsync("SendSubmitUsernameChange", user, username, password);
        }

        public async Task SubmitEmailChange(UserModel user, string email, string password)
        {
            await Connection.SendAsync("SendSubmitEmailChange", user, email, password);
        }

        public async Task SubmitPasswordChange(UserModel user, string newPassword, string currentPassword)
        {
            await Connection.SendAsync("SendSubmitPasswordChange", user, newPassword, currentPassword);
        }
    }
}
