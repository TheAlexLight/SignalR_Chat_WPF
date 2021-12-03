using ChatClient.Commands.AuthenticationCommands;
using ChatClient.Commands.NavigatonCommands;
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
    public class RegistrationViewModel : ChatViewModelBase, IDataErrorInfo/*, INotifyDataErrorInfo*/
    {
        public RegistrationViewModel(ChatBaseConfiguration baseConfiguration) : base(baseConfiguration)
        {
            Window window = Application.Current.MainWindow;

            if (window != null)
            {
               window = BaseConfiguration.WindowConfigurationService.SetWindowStartupData(window: window, width: 385, height: 545
                    , left: window.Left, top: window.Top, minWidth: 385, minHeight: 545);
            }

            NavigateLoginCommand = new NavigateCommand<LoginViewModel>(BaseConfiguration, BaseConfiguration);

            RegistrationCommand = new RegistrationCommand(this);


            //if (!BaseConfiguraction.ChatService.IsEventHandlerRegistered())
            //{
                BaseConfiguration.ChatService.AuthorizationModel.RegistrationResultReceived += ChatService_ReceiveRegistrationResult;
            //}
            

             
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
                MessageBox.Show("Registration was successful");

                BaseConfiguration.CreateConcreteNavigationService<LoginViewModel>(BaseConfiguration);

                BaseConfiguration.NavigationService.Navigate();
            }
            else
            {
                MessageBox.Show(error);
            }
        }
    }
}

