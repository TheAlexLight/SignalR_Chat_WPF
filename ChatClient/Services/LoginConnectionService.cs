using ChatClient.Enums;
using ChatClient.Stores;
using ChatClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Services
{
    public class LoginConnectionService
    {
        public NavigationStore NavigationStore { get; }
        public  SignalRChatService ChatService { get; }

        public LoginConnectionService(NavigationStore navigationStore, SignalRChatService chatService)
        {
            NavigationStore = navigationStore;
            ChatService = chatService;
        }

        public ChatViewModelBase CreateConnectedViewModel (SignalRChatService chatService, ChatViewModelBase viewModelType)
        {
            ChatViewModelBase viewModel = viewModelType;

            chatService.Connect().ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    viewModel.ErrorMessage = "Unable to connect to chat hub";
                    viewModel.ConnectionStatusValue = ConnectionStatus.Disconnected;
                }
                else
                {
                    viewModel.ConnectionStatusValue = ConnectionStatus.Connected;
                }
            });

            return viewModel;
        }
    }
}
