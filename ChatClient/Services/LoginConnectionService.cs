using ChatClient.Enums;
using ChatClient.Interfaces;
using ChatClient.Stores;
using ChatClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.Services
{
    public class LoginConnectionService
    {
        public INavigator Navigator { get; }
        public ISignalRChatService ChatService { get; }

        public LoginConnectionService(INavigator navigator, ISignalRChatService chatService)
        {
            Navigator = navigator;
            ChatService = chatService;
        }

        public async Task<ChatViewModelBase> CreateConnectedViewModel (ISignalRChatService chatService, ChatViewModelBase viewModelType)
        {
            ChatViewModelBase viewModel = viewModelType;

           await chatService.Connect().ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    viewModel.ErrorMessage = "Unable to connect to chat hub";
                    MessageBox.Show("Unable to connect to server");
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
