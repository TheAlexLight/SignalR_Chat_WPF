using ChatClient.Commands;
using ChatClient.Commands.AuthenticationCommands;
using ChatClient.Commands.NavigatonCommands;
using ChatClient.Interfaces;
using ChatClient.Services;
using ChatClient.Stores;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ChatClient.ViewModels
{
    public class RegistrationViewModel : ChatViewModelBase, IDataErrorInfo/*, INotifyDataErrorInfo*/
    {
        public RegistrationViewModel(INavigator navigator, ISignalRChatService chatService
               , IWindowConfigurationService windowConfigurationService) : base(navigator, chatService, windowConfigurationService)
        {
            Window window = Application.Current.MainWindow;

            if (window != null)
            {
                windowConfigurationService.SetWindowStartupData(window: window, width: 385, height: 545);
            }

            NavigateLoginCommand = new NavigateCommand<LoginViewModel>(
                    new NavigationService<LoginViewModel>(Navigator,
                    () => new LoginViewModel(Navigator, chatService, windowConfigurationService)));

            RegistrationCommand = new RegistrationCommand(this);

            chatService.ReceiveRegistrationResult += ChatService_ReceiveRegistrationResult;
        }

        public ICommand NavigateLoginCommand { get; }
        public ICommand RegistrationCommand { get; }

        private string _username;
        private string _email;
        private string _password;
        private string _passwordConfirm;

        //public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
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
                OnPropertyChanged(nameof(PasswordConfirm));
            }
        }

        public string PasswordConfirm
        {
            get => _passwordConfirm;
            set
            {
                _passwordConfirm = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Password));
            }
        }

        public string Error { get; }

        public string this[string propertyName]
        {
            get
            {
                string result = string.Empty;

                propertyName ??= string.Empty;

                if (propertyName != string.Empty
                        && (propertyName == nameof(PasswordConfirm) || propertyName == nameof(Password))
                        && PasswordConfirm != null && Password != null)
                {
                    if (!Password.Equals(PasswordConfirm, StringComparison.Ordinal))
                    {
                        result = "Passwords do not match";
                    }
                }

                return result;
            }
        }

        private void ChatService_ReceiveRegistrationResult(bool registrationResult, string error)
        {
            if (registrationResult)
            {
                MessageBox.Show("Registration Succeded");
                NavigationService<LoginViewModel> navigationService = new(Navigator,
                        () => new LoginViewModel(Navigator, ChatService, WindowConfigurationService));
                navigationService.Navigate();
            }
            else
            {
                MessageBox.Show(error);
            }
        }
    }
}

