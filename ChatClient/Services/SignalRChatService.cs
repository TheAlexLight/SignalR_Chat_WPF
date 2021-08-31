
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR.Client;
using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Services
{
    public class SignalRChatService
    {
        public HubConnection Connection { get; }

        public event Action<bool, string> ReceiveRegistrationResult;
        public event Action<bool> ReceiveLoginResult;
        public event Action<UserProfileModel> CurrentUserReceived;
        public event Action<ObservableCollection<UserProfileModel>> UserListReceived;
        public event Action<List<MessageModel>> SavedMessagesReceived;
        public event Action<MessageModel> MessageReceived;
        public event Action<bool> ReceivedBan;
        public event Action ReceivedKick;

        public SignalRChatService(HubConnection connection)
        {
            Connection = connection;

            GetChatHubMessagesInvoke();
        }

        private void GetChatHubMessagesInvoke()
        {
            Connection.On<bool, string>("ReceiveRegistrationResult", (result, error) => ReceiveRegistrationResult?.Invoke(result, error));
            Connection.On<bool>("ReceiveLoginResult", (result) => ReceiveLoginResult?.Invoke(result));
            Connection.On<ObservableCollection<UserProfileModel>>("ReceiveUserList", (activeUsers) => UserListReceived?.Invoke(activeUsers));
            Connection.On<List<MessageModel>>("ReceiveSavedMessages", (messages) => SavedMessagesReceived?.Invoke(messages));
            Connection.On<UserProfileModel>("ReceiveCurrentUser", (currentUser) => CurrentUserReceived?.Invoke(currentUser));
            Connection.On<MessageModel>("ReceiveMessage", (messageModel) => MessageReceived?.Invoke(messageModel));
            Connection.On<bool>("ReceiveBan", (result) => ReceivedBan?.Invoke(result));
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

        public async Task SendMessage(MessageModel message)
        {
            await Connection.SendAsync("SendMessage", message);
        }

        public async Task Registration(UserRegistrationModel model)
        {
            await Connection.SendAsync("SendRegistration", model);
        }

        public async Task Login(UserLoginModel model)
        {
            await Connection.SendAsync("SendLogin", model);
        }

        public async Task SendBan(string username)
        {
            await Connection.SendAsync("SendUserBan", username);
        }

        public async Task KickUser(string username)
        {
            await Connection.SendAsync("SendKickUser", username);
        }
    }
}
