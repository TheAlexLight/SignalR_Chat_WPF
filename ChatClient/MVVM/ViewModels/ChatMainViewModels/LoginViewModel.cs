using ChatClient.Commands;
using ChatClient.Commands.AuthenticationCommands;
using ChatClient.Commands.NavigatonCommands;
using ChatClient.Enums;
using ChatClient.Interfaces.Authorization;
using ChatClient.MVVM.ViewModels.BaseViewModels;
using ChatClient.Services.BaseConfiguration;
using ChatClient.Services.ConcreteConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChatClient.MVVM.ViewModels.ChatMainViewModels
{
   public class LoginViewModel : ChatViewModelBase
    {
        public LoginViewModel(ChatBaseConfiguration baseConfiguration) : base(baseConfiguration)
        {
            UserCredentials = new UserCredentialsViewModel();

            BaseConfiguration.ChatService.AuthorizationModel.LoginResultReceived += ChatService_ReceiveLoginResult;

            LoginCommand = new LoginCommand(this);

            NavigateRegistrationCommand = new NavigateCommand<RegistrationViewModel>(baseConfiguration, baseConfiguration);

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

        public ILogin UserCredentials { get; set; }

        public CommandBase LoginCommand { get; }

        public ICommand NavigateRegistrationCommand { get; }

        private void ChatService_ReceiveLoginResult(bool loginResult)
        {
            if (loginResult)
            {
                BaseConfiguration.CreateConcreteNavigationService<ChatViewModel>(BaseConfiguration);

                BaseConfiguration.NavigationService.Navigate();
            }
            else
            {
                MessageBox.Show("Username or password can't be found");
            }

            IsLoading = false;
            LoginCommand.RaiseCanExecuteChanged();
        }
    }
}
