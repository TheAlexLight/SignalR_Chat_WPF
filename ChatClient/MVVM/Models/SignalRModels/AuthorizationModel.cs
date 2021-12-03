using ChatClient.Interfaces.BaseConfiguration;
using ChatClient.Interfaces.SignalRTransmitting;
using ChatClient.Interfaces.SignalRTransmitting.SignalTranmittingResult;
using Microsoft.AspNetCore.SignalR.Client;
using SharedItems.Models;
using SharedItems.Models.AuthenticationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.MVVM.Models.SignalRModels
{
    public class AuthorizationModel : IAuthorizationResult
    {
        private readonly IEventHandler _chatService;

        public AuthorizationModel(IEventHandler chatService)
        {
            _chatService = chatService;

            _chatService.Connection.On<bool, string>("ReceiveRegistrationResult", (result, error) => _registrationResultReceived?.Invoke(result, error));
            _chatService.Connection.On<bool>("ReceiveLoginResult", (result) => _loginResultReceived?.Invoke(result));
        }

        private  Action<bool, string> _registrationResultReceived;
        private  Action<bool> _loginResultReceived;

        public event Action<bool, string> RegistrationResultReceived
        {
            add
            {
                if (!_chatService.IsEventHandlerRegistered(_registrationResultReceived, value))
                {
                    _registrationResultReceived += value;
                }
            }

            remove => _registrationResultReceived -= value;
        }

        public event Action<bool> LoginResultReceived
        {
            add
            {
                if (!_chatService.IsEventHandlerRegistered(_loginResultReceived, value))
                {
                    _loginResultReceived += value;
                }
            }

            remove => _loginResultReceived -= value;
        }

        public async Task Connect()
        {
            await _chatService.Connection.StartAsync();
        }

        public async Task Reconnect(string username)
        {
            await _chatService.Connection.SendAsync("SendReconnection", username);
        }

        public async Task Register(UserRegistrationModel model)
        {
            await _chatService.Connection.SendAsync("SendRegistration", model);
        }

        public async Task Login(UserLoginModel model)
        {
            await _chatService.Connection.SendAsync("SendLogin", model);
        }
    }
}
