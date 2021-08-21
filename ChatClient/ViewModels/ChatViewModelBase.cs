using ChatClient.Enums;
using ChatClient.Services;
using ChatClient.Stores;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.ViewModels
{
   public class ChatViewModelBase : ViewModelBase
    {
        protected readonly SignalRChatService _chatService;
        protected readonly NavigationStore _navigationStore;

        public ChatViewModelBase(SignalRChatService chatService, NavigationStore navigationStore)
        {
            _chatService = chatService;
            _navigationStore = navigationStore;

            _chatService.Connection.Reconnecting += Connection_Reconnecting;
        }

        protected virtual Task Connection_Reconnecting(Exception arg)
        {
            throw new NotImplementedException(); //ToDo: implement
        }

        private string _errorMessage;
        private ConnectionStatus _connectionStatusValue;

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }

        public ConnectionStatus ConnectionStatusValue
        {
            get => _connectionStatusValue;
            set
            {
                _connectionStatusValue = value;
                OnPropertyChanged();
            }
        }

        public async Task<HubConnectionState> ConnectToServer(ChatViewModelBase viewModel)
        {
            if (_chatService.Connection.State == HubConnectionState.Disconnected)
            {
                 LoginConnectionService connectionService = new(_navigationStore, _chatService);

                _navigationStore.CurrentViewModel = await connectionService.CreateConnectedViewModel(_chatService, viewModel);
            }

            return _chatService.Connection.State;
        }
    }
}
