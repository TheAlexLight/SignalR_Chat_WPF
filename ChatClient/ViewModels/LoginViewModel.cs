using ChatClient.Commands;
using ChatClient.Commands.AuthenticationCommands;
using ChatClient.Enums;
using ChatClient.Services;
using ChatClient.Stores;
using ChatClient.Views;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChatClient.ViewModels
{
   public class LoginViewModel : ChatViewModelBase
    {
        public LoginViewModel(NavigationStore navigationStore, SignalRChatService chatService) : base(chatService, navigationStore)
        {
            ChatService = chatService;
            _chatViewModel = new ChatViewModel(ChatService, _navigationStore);

            _chatService.ConnectionReceived += ChatService_ConnectionReceived;
            _chatService.ReceiveLoginResult += ChatService_ReceiveLoginResult;

            LoginCommand = new LoginCommand(this);
            NavigateRegistrationCommand = new NavigateCommand<RegistrationViewModel>
                    (new NavigationService<RegistrationViewModel>(navigationStore,
                    () => new RegistrationViewModel(navigationStore, chatService)));

            ConnectionStatusValue = ConnectionStatus.Connecting;

            Window window = Application.Current.MainWindow;

            if (window != null)
            {
                window.Height = 385;
                window.Width = 385;
            }
        }

        private ChatViewModel _chatViewModel;
        public SignalRChatService ChatService { get; }

        private string _username;
        private string _password;
        private bool _isLoading;

        public LoginCommand LoginCommand { get; }
        public ICommand NavigateRegistrationCommand { get; }

        private void ChatService_ConnectionReceived(bool isConnected)
        {
            UserStatusService.IsLogin = isConnected;
            LoginCommand.RaiseCanExecuteChanged();
        }

        private void ChatService_ReceiveLoginResult(bool loginResult)
        {
            if (loginResult)
            {
                NavigationService<ChatViewModel> navigationService = new(_navigationStore,
                       () => new ChatViewModel(_chatService, _navigationStore));
                navigationService.Navigate();
            }
            else
            {
                MessageBox.Show("Username or password can't be found");
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
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
                LoginCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
