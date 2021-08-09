using Microsoft.AspNetCore.SignalR.Client;
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

        public event Action<string> MessageReceived;
        public event Action<bool> TryLogin;
        public event Action<bool> ConnectionReceived;
        public event Action<bool> ReceivedBan;
        public event Action<ObservableCollection<string>> UserListReceived;

        public SignalRChatService(HubConnection connection)
        {
            Connection = connection;

            GetChatHubMessagesInvoke();
        }

        private void GetChatHubMessagesInvoke()
        {
            Connection.On<string>("ReceiveMessage", (message) => MessageReceived?.Invoke(message));
            Connection.On<bool>("TryLogin", (result) => TryLogin?.Invoke(result));
            Connection.On<bool>("ReceiveConnectionInfo", (result) => ConnectionReceived?.Invoke(result));
            Connection.On<bool>("ReceiveBan", (result) => ReceivedBan?.Invoke(result));
            Connection.On<ObservableCollection<string>>("ReceiveUserList", (activeUsers) => UserListReceived?.Invoke(activeUsers));
        }

        public async Task Connect()
        {
            await Connection.StartAsync();
        }

        public async Task SendMessage(string message)
        {
            await Connection.SendAsync("SendMessage", message);
        }

        public async Task Login(string username)
        {
            await Connection.SendAsync("SendLogin", username);
        }

        public async Task SendBan(string username)
        {
            await Connection.SendAsync("SendUserBan", username);
        }
    }
}
