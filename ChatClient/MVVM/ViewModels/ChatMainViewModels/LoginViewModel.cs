using ChatClient.Commands;
using ChatClient.Commands.AuthenticationCommands;
using ChatClient.Commands.NavigatonCommands;
using ChatClient.Enums;
using ChatClient.Interfaces;
using ChatClient.Services;
using ChatClient.Services.BaseConfiguration;
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
        public LoginViewModel(ChatBaseConfiguration baseConfiguration) : base(baseConfiguration)
        {
            BaseConfiguration.ChatService.ReceiveLoginResult += ChatService_ReceiveLoginResult;

            LoginCommand = new LoginCommand(this);

            //NavigateRegistrationCommand = new NavigateCommand<RegistrationViewModel>
            //        (new NavigationService<RegistrationViewModel>(navigator,
            //        () => new RegistrationViewModel(navigator, chatService, WindowConfigurationService)));

            NavigateRegistrationCommand = new NavigateCommand<RegistrationViewModel>
                   (new NavigationService<RegistrationViewModel>(BaseConfiguration.Navigator,
                   () => new RegistrationViewModel(BaseConfiguration)));

            //navigator.CurrentViewModel = new RegistrationViewModel(navigator, chatService, WindowConfigurationService); ;

            //NavigationService<RegistrationViewModel> vv = new(navigator,
            //        () => new RegistrationViewModel(navigator, chatService, WindowConfigurationService));

            //vv.Navigate();

            ConnectionStatusValue = ConnectionStatus.Connecting;

            Window window = Application.Current.MainWindow;

            if (window != null)
            {
                BaseConfiguration.WindowConfigurationService.SetWindowStartupData(window: window, width: 385, height: 385
                    , left: window.Left, top: window.Top, minWidth: 385, minHeight: 385);

                window.MaxHeight = 385;
                window.MaxHeight = double.PositiveInfinity;
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
                NavigationService<ChatViewModel> navigationService = new(BaseConfiguration.Navigator,
                       () => new ChatViewModel(BaseConfiguration));
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
