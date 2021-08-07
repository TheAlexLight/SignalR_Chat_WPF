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
        private readonly NavigationStore _navigation;
        private readonly SignalRChatService _chatService;

        public LoginConnectionService(NavigationStore navigation, SignalRChatService chatService)
        {
            _navigation = navigation;
            _chatService = chatService;
        }

        public LoginViewModel CreateConnectedViewModel(SignalRChatService chatService)
        {
            LoginViewModel viewModel = new LoginViewModel(_navigation, _chatService);

            chatService.Connect().ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    viewModel.ErrorMessage = "Unable to connect to chat hub";
                    viewModel.ConnectionStatus = "Disconnected";
                }
                else
                {
                    viewModel.ConnectionStatus = "Connected";
                }
            });

            return viewModel;
        }
    }
}
