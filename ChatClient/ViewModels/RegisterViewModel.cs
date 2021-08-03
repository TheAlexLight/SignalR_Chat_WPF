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
    public class RegisterViewModel : ViewModelBase
    {
        public RegisterViewModel(NavigationStore navigationStore)
        {
            NavigateLoginCommand = new NavigateCommand<LoginViewModel>(new NavigationService<LoginViewModel>(navigationStore, 
                    () => new LoginViewModel(navigationStore)));
        }

        private string username;
        private string password;

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
