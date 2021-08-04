using ChatClient.Commands;
using ChatClient.Encryption;
using ChatClient.Services;
using ChatClient.Services.Interfaces;
using ChatClient.Stores;
using ChatClient.Views;
using EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChatClient.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        public RegisterViewModel(NavigationStore navigationStore)
        {
            NavigateLoginCommand = new NavigateCommand<LoginViewModel>(new NavigationService<LoginViewModel>(navigationStore, 
                    () => new LoginViewModel(navigationStore)));
        }

        private string username;
        private string password;

        private ICommand registerCommand;
        public ICommand RegisterCommand { get {
                return registerCommand ??= new RelayCommand(parameter =>
                 {
                     IAuthenticationService authentication = new AuthenticationService(new AccountDataService<Account>(), new PasswordHasher());
                     authentication.Register("qwer@gmail.com", Username, Password, Password);
                 });
                    } 
        }
        public ICommand NavigateLoginCommand { get; }

        public string Username
        {
            get => username;
            set 
            {
                username = value;
                OnPropertyChanged();
            } 
        }
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }
    }
}
