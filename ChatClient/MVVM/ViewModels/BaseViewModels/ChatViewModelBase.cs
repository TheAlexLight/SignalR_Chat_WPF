using ChatClient.Commands;
using ChatClient.Enums;
using ChatClient.Services;
using ChatClient.Services.BaseConfiguration;
using Microsoft.AspNetCore.SignalR.Client;
using SharedItems.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatClient.MVVM.ViewModels.BaseViewModels
{
   public class ChatViewModelBase : ViewModelBase
    {
        public ChatBaseConfiguration BaseConfiguration { get; }

        public ChatViewModelBase(ChatBaseConfiguration baseConfiguration)
        {
            BaseConfiguration = baseConfiguration;

            BaseConfiguration.ChatService.Connection.Reconnecting += Connection_Reconnecting;
            BaseConfiguration.ChatService.Connection.Reconnected += Connection_Reconnected;
            BaseConfiguration.ChatService.Connection.Closed += Connection_Closed;
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
            if (BaseConfiguration.ChatService.Connection.State == HubConnectionState.Disconnected)
            {
                 LoginConnectionService connectionService = new(BaseConfiguration.Navigator, BaseConfiguration.ChatService);

                BaseConfiguration.Navigator.CurrentViewModel = await connectionService.CreateConnectedViewModel(BaseConfiguration.ChatService, viewModel);
            }

            return BaseConfiguration.ChatService.Connection.State;
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
