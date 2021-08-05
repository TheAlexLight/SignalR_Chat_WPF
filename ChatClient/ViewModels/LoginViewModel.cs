using ChatClient.Commands;
using ChatClient.Services;
using ChatClient.Stores;
using ChatClient.Views;
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
        public LoginViewModel(NavigationStore navigationStore, SignalRChatService chatService)
        {
            ChatService = chatService;
            _navigationStore = navigationStore;
            chatService.TryLogin += ChatService_TryLogin;

            LoginCommand = new LoginCommand(this, new NavigationService<ChatViewModel>(navigationStore, 
                    () => new ChatViewModel(ChatService)/*ChatViewModel.CreateConnectedViewModel(chatService)*/));
        }

        private void ChatService_TryLogin(bool usernameExist)
        {
            if (!usernameExist)
            {
                NavigationService<ChatViewModel> navigationService = new(_navigationStore, () => new ChatViewModel(ChatService));
                navigationService.Navigate();
            }
            else
            {
                MessageBox.Show("Username is already exist");
            }
           
        }

        private NavigationStore _navigationStore;
        public SignalRChatService ChatService { get;}

        private string username;

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
    }
}
