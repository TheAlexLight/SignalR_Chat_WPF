using ChatClient.Commands;
using ChatClient.Commands.AuthenticationCommands;
using ChatClient.Enums;
using ChatClient.Interfaces;
using ChatClient.Services;
using ChatClient.Stores;
using Microsoft.AspNetCore.SignalR.Client;
using SharedItems.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatClient.ViewModels
{
   public class ChatViewModelBase : ViewModelBase
    {
        public ISignalRChatService ChatService { get; }
        public INavigator Navigator { get; }
        public IWindowConfigurationService WindowConfigurationService { get; }

        public ChatViewModelBase(INavigator navigator, ISignalRChatService signalChatRService
                , IWindowConfigurationService windowConfigurationServie)
        {
            ChatService = signalChatRService;
            Navigator = navigator;
            WindowConfigurationService = windowConfigurationServie;

            ChatService.Connection.Reconnecting += Connection_Reconnecting;
            ChatService.Connection.Reconnected += Connection_Reconnected;
            ChatService.Connection.Closed += Connection_Closed;
        }

        private string _errorMessage;
        private bool _isLoading;
        private ConnectionStatus _connectionStatusValue;

        public CommandBase ReconnectionCommand { get; set; }

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

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public async Task<HubConnectionState> ConnectToServer(ChatViewModelBase viewModel)
        {
            if (ChatService.Connection.State == HubConnectionState.Disconnected)
            {
                 LoginConnectionService connectionService = new(Navigator, ChatService);

                Navigator.CurrentViewModel = await connectionService.CreateConnectedViewModel(ChatService, viewModel);
            }

            return ChatService.Connection.State;
        }

        protected virtual Task Connection_Reconnecting(Exception arg)
        {
            ConnectionStatusValue = ConnectionStatus.Reconnecting;
            IsLoading = true;

            if (ReconnectionCommand != null)
            {
                ReconnectionCommand.RaiseCanExecuteChanged();
            }

            return Task.CompletedTask;
        }

        private async Task Connection_Reconnected(string arg)
        {
            await Reconnect();

            ConnectionStatusValue = ConnectionStatus.Connected;
            IsLoading = false;

            if (ReconnectionCommand != null)
            {
                ReconnectionCommand.RaiseCanExecuteChanged();
            }
        }

        protected virtual async Task Reconnect()
        {
           await Task.CompletedTask;
        }

        private Task Connection_Closed(Exception arg)
        {
            ConnectionStatusValue = ConnectionStatus.Disconnected;
            IsLoading = false;

            if (ReconnectionCommand != null)
            {
                ReconnectionCommand.RaiseCanExecuteChanged();
            }

            return Task.CompletedTask;
        }
    }
}
