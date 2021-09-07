using ChatClient.Commands;
using ChatClient.Commands.AuthenticationCommands;
using ChatClient.Commands.NavigatonCommands;
using ChatClient.Enums;
using ChatClient.Interfaces;
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
        public LoginViewModel(INavigator navigator, ISignalRChatService chatService
                , IWindowConfigurationService windowConfigurationService) : base(navigator, chatService, windowConfigurationService)
        {
            base.ChatService.ReceiveLoginResult += ChatService_ReceiveLoginResult;

            LoginCommand = new LoginCommand(this);
            NavigateRegistrationCommand = new NavigateCommand<RegistrationViewModel>
                    (new NavigationService<RegistrationViewModel>(navigator,
                    () => new RegistrationViewModel(navigator, chatService, WindowConfigurationService)));

            ConnectionStatusValue = ConnectionStatus.Connecting;

            Window window = Application.Current.MainWindow;

            if (window != null)
            {
                windowConfigurationService.SetWindowStartupData(window: window, width: 385, height: 385);
            }
        }

        private string _username;
        private string _password;

        public CommandBase LoginCommand { get; }
        public ICommand NavigateRegistrationCommand { get; }

        private void ChatService_ReceiveLoginResult(bool loginResult)
        {
            if (loginResult)
            {
                NavigationService<ChatViewModel> navigationService = new(Navigator,
                       () => new ChatViewModel(Navigator, ChatService, WindowConfigurationService));
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
    }
}
