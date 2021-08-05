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
            _chatViewModel = new ChatViewModel(ChatService);
            chatService.TryLogin += ChatService_TryLogin;
            chatService.ConnectionReceived += ChatService_ConnectionReceived; ;

            LoginCommand = new LoginCommand(this);

            ConnectionStatus = "Connection...";
        }

        private void ChatService_ConnectionReceived(bool isConnected)
        {
            LoginConnectionService.IsLogin = isConnected;
            LoginCommand.RaiseCanExecuteChanged();
        }

        private void ChatService_TryLogin(bool usernameExist)
        {


            if (!usernameExist)
            {
                NavigationService<ChatViewModel> navigationService = new(_navigationStore, () => _chatViewModel);
                navigationService.Navigate();
            }
            else
            {
                MessageBox.Show("Username is already exist");
            }
           
        }

        private NavigationStore _navigationStore;
        private ChatViewModel _chatViewModel;
        public SignalRChatService ChatService { get;}

        private string username;

        public LoginCommand LoginCommand { get; }

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
