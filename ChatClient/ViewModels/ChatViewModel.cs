using ChatClient.Commands;
using ChatClient.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChatClient.ViewModels
{
    public class ChatViewModel : ChatViewModelBase
    {
        public ChatViewModel()
        {

        }
        public ChatViewModel(SignalRChatService chatService)
        {
            SendChatMessageCommand = new SendChatCommand(this, chatService);

            Messages = new();

            chatService.MessageReceived += ChatService_MessageReceived;
            chatService.UserListReceived += ChatService_UserListReceived;
        }

        private void ChatService_UserListReceived(ObservableCollection<string> activeUsers)
        {
            ActiveUsers = activeUsers;
        }

        private string _message;
        private ObservableCollection<string> _activeUsers;

        public ObservableCollection<string> Messages { get; }

        public ICommand SendChatMessageCommand { get; }

        public static ChatViewModel CreateConnectedViewModel(SignalRChatService chatService)
        {
            ChatViewModel viewModel = new(chatService);

            chatService.Connect().ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    viewModel.ErrorMessage = "Unable to connect to chat hub";
                }
            });

            return viewModel;
        }

        private void ChatService_MessageReceived(string message)
        {
            Messages.Add(message);
        }

        private void ChatService_NameReceived(string name)
        {
            MessageBox.Show(name);
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> ActiveUsers
        {
            get => _activeUsers;
            set
            {
                _activeUsers = value;
                OnPropertyChanged();
            }
        }
    }
}
