using ChatClient.Commands;
using ChatClient.Commands.AuthenticationCommands;
using ChatClient.Commands.NavigatonCommands;
using ChatClient.Interfaces.Authorization;
using ChatClient.MVVM.ViewModels.BaseViewModels;
using ChatClient.Services.BaseConfiguration;
using ChatClient.Services.ConcreteConfiguration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ChatClient.MVVM.ViewModels.ChatMainViewModels
{
    public class RegistrationViewModel : ChatViewModelBase
    {
        public RegistrationViewModel(ChatBaseConfiguration baseConfiguration) : base(baseConfiguration)
        {
            Window window = Application.Current.MainWindow;

            if (window != null)
            {
                window = BaseConfiguration.WindowConfigurationService.SetWindowStartupData(window: window, width: 385, height: 545
                     , left: window.Left, top: window.Top, minWidth: 385, minHeight: 545);
            }

            UserCredentials = new UserCredentialsViewModel();

            NavigateLoginCommand = new NavigateCommand<LoginViewModel>(BaseConfiguration, BaseConfiguration);

            RegistrationCommand = new RegistrationCommand(this);

            BaseConfiguration.ChatService.AuthorizationModel.RegistrationResultReceived += ChatService_ReceiveRegistrationResult;
        }

        public ICommand NavigateLoginCommand { get; }

        public CommandBase RegistrationCommand { get; }

        public UserCredentialsViewModel UserCredentials { get; set; }

        private void ChatService_ReceiveRegistrationResult(bool registrationResult, string error)
        {
            if (registrationResult)
            {
                MessageBox.Show("Registration was successful");

                BaseConfiguration.CreateConcreteNavigationService<LoginViewModel>(BaseConfiguration);

                BaseConfiguration.NavigationService.Navigate();
            }
            else
            {
                MessageBox.Show(error);
            }

            IsLoading = false;

            RegistrationCommand.RaiseCanExecuteChanged();
        }
    }
}

