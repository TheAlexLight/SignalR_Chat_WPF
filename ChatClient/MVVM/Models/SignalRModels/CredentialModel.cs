using ChatClient.Interfaces.SignalRTransmitting;
using ChatClient.Interfaces.SignalRTransmitting.SignalTranmittingResult;
using Microsoft.AspNetCore.SignalR.Client;
using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.MVVM.Models.SignalRModels
{
    public class CredentialModel : ICredentialResult
    {
        private readonly IEventHandler _chatService;

        public CredentialModel(IEventHandler chatService)
        {
            _chatService = chatService;

            _chatService.Connection.On<bool, string, string>("ReceiveUserPropertyChange", (success, propertyName, message) => _userCredentialsResultReceived?.Invoke(success, propertyName, message));
        }

        private Func<bool, string, string, Task> _userCredentialsResultReceived;

        public event Func<bool, string, string, Task> UserCredentialsResultReceived
        {
            add
            {
                if (!_chatService.IsEventHandlerRegistered(_userCredentialsResultReceived, value))
                {
                    _userCredentialsResultReceived += value;
                }
            }

            remove => _userCredentialsResultReceived -= value;
        }

        public async Task ChangeUsername(UserModel user, string username, string password)
        {
            await _chatService.Connection.SendAsync("SendSubmitUsernameChange", user, username, password);
        }

        public async Task ChangeEmail(UserModel user, string email, string password)
        {
            await _chatService.Connection.SendAsync("SendSubmitEmailChange", user, email, password);
        }

        public async Task ChangePassword(UserModel user, string newPassword, string currentPassword)
        {
            await _chatService.Connection.SendAsync("SendSubmitPasswordChange", user, newPassword, currentPassword);
        }
    }
}
