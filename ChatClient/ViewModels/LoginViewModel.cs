using ChatClient.Commands;
using ChatClient.Services;
using ChatClient.Stores;
using ChatClient.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChatClient.ViewModels
{
   public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel(NavigationStore navigationStore)
        {
            NavigateRegisterCommand = new NavigateCommand<RegisterViewModel>(new NavigationService<RegisterViewModel>(navigationStore, 
                    () => new RegisterViewModel(navigationStore)));
            LoginCommand = new LoginCommand(this, new NavigationService<ChatViewModel>(navigationStore, () => new ChatViewModel()));
        }

        private string username;
        private string password;

        public ICommand NavigateRegisterCommand { get; }
        public ICommand LoginCommand { get; }

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
