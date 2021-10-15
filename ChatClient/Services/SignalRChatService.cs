using ChatClient.Enums;
using ChatClient.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SharedItems.Enums;
using SharedItems.Models;
using SharedItems.Models.AuthenticationModels;
using SharedItems.Models.StatusModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
        public event Action<string> CurrentGroupReceived;
        //public event Action<MessageModel> MessageReceived;
        public event Action ReceivedKick;
        public event Action<BanStatusModel> ReceivedBan;
        public event Action<MuteStatusModel> ReceivedMute;

        public SignalRChatService(HubConnectionBuilder connectionBuilder)
        {
            Connection = connectionBuilder
                    .WithUrl("http://localhost:5000/chat")
                    .WithAutomaticReconnect()
                    .Build(); ;

            GetChatHubMessagesInvoke();
        }

        private void GetChatHubMessagesInvoke()
        {
            Connection.On<bool, string>("ReceiveRegistrationResult", (result, error) => ReceiveRegistrationResult?.Invoke(result, error));
            Connection.On<bool>("ReceiveLoginResult", (result) => ReceiveLoginResult?.Invoke(result));
            Connection.On<ObservableCollection<UserModel>>("ReceiveUserList", (activeUsers) => UserListReceived?.Invoke(activeUsers));
            Connection.On<string>("ReceiveCurrentGroup", (group) => CurrentGroupReceived?.Invoke(group));
            Connection.On<UserModel>("ReceiveCurrentUser", (currentUser) => CurrentUserReceived?.Invoke(currentUser));
            //Connection.On<MessageModel>("ReceiveMessage", (messageModel) => MessageReceived?.Invoke(messageModel));
            Connection.On<BanStatusModel>("ReceiveBan", (model) => ReceivedBan?.Invoke(model));
            Connection.On<MuteStatusModel>("ReceiveMute", (model) => ReceivedMute?.Invoke(model));
            Connection.On("ReceiveKick", () => ReceivedKick?.Invoke());
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
            string messageJsonString = JsonConvert.SerializeObject(message);
            await Connection.SendAsync("SendMessage", messageJsonString, currentGroup, selectedUser, currentUser);
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
        
        public async Task SwitchChat(ChatType chatTtype)
        {
            await Connection.SendAsync("SendSwitchChat", chatTtype);
        }
        public async Task UpdatePrivateMessages(UserModel selectedUser, UserModel currentUser)
        {
            await Connection.SendAsync("SendUpdatePrivateMEssages", selectedUser, currentUser);
        }

        public async Task ChangePhoto(UserModel currentUser, byte[] photo)
        {
            await Connection.SendAsync("SendChangePhoto", currentUser, photo);
        }
    }
}
