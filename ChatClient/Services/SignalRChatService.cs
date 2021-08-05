using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Services
{
    public class SignalRChatService
    {
        private readonly HubConnection _connection;

        public event Action<string> MessageReceived;
        public event Action<bool> TryLogin;

        public SignalRChatService(HubConnection connection)
        {
            _connection = connection;

            _connection.On<string>("ReceiveMessage", (message) => MessageReceived?.Invoke(message));
            _connection.On<bool>("TryLogin", (result) => TryLogin?.Invoke(result));
        }

        public async Task Connect()
        {
            await _connection.StartAsync();
        }

        public async Task SendMessage(string message)
        {
            await _connection.SendAsync("SendMessage", message);
        }

        public async Task Login(string username)
        {
            await _connection.SendAsync("SendLogin", username);
        }
    }
}
