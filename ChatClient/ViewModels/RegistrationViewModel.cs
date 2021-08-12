using ChatClient.Commands;
using ChatClient.Commands.AuthenticationCommands;
using ChatClient.Services;
using ChatClient.Stores;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChatClient.ViewModels
{
    public class RegistrationViewModel : ChatViewModelBase, INotifyDataErrorInfo
    {
        public RegistrationViewModel(NavigationStore navigationStore, SignalRChatService chatService) : base(chatService)
        {
            Window window = Application.Current.MainWindow;
            window.Height = 550;

            _navigationStore = navigationStore;

            NavigateLoginCommand = new NavigateCommand<LoginViewModel>(
                    new NavigationService<LoginViewModel>(_navigationStore,
                    () => new LoginViewModel(_navigationStore, chatService)));

            RegistrationCommand = new RegistrationCommand(this, chatService);

            chatService.ReceiveRegistrationResult += ChatService_ReceiveRegistrationResult;
        }

        public ICommand NavigateLoginCommand { get; }
        public ICommand RegistrationCommand { get; }

        private string _username;
        private string _email;
        private string _password;
        private string _passwordConfirm;

        private readonly NavigationStore _navigationStore;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                if (String.IsNullOrEmpty(value))
                {
                    throw new ApplicationException("Customer name is mandatory.");
                }
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
            }
        }

        public string PasswordConfirm
        {
            get => _passwordConfirm;
            set
            {
                _passwordConfirm = value;
                OnPropertyChanged();
            }
        }

        private void ChatService_ReceiveRegistrationResult(IdentityResult registrationResult)
        {
            HasErrors = !registrationResult.Succeeded;
            if (!HasErrors)
            {
                NavigationService<ChatViewModel> navigationService = new(_navigationStore, 
                        () => new ChatViewModel(_chatService));
                navigationService.Navigate();
            }
            else
            {
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Password)));
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Username)));
            }
            //else
            //{
            //    MessageBox.Show(registrationResult.Errors);
            //}
        }

        public bool HasErrors { get; set; }

        public IEnumerable GetErrors(string propertyName)
        {
            return string.IsNullOrWhiteSpace(propertyName) || (!HasErrors) ? null
                    : new List<string>()
                    {
                        "Invalid credentials"
                    };
        }
    }
}
