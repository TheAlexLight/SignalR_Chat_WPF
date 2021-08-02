using ChatClient.Commands;
using ChatClient.Model;
using ChatClient.Operations;
using ChatClient.Stores;
using ChatClient.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace ChatClient.ViewModels
{
   public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel(NavigationStore navigationStore)
        {
            NavigateRegisterCommand = new NavigateRegisterCommand(navigationStore);
        }

        private string username;
        private string password;

        public ICommand NavigateRegisterCommand { get; }

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
