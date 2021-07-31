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

        public SignalRChatService(HubConnection connection)
        {
            _connection = connection;

            _connection.On<string>("ReceiveMessage", (message) => MessageReceived?.Invoke(message));
        }

        public async Task Connect()
        {
            await _connection.StartAsync();
        }

        public async Task SendMessage(string message)
        {
            await _connection.SendAsync("SendMessage", message);
        }
    }
}
