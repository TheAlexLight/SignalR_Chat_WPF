using ChatClient.Commands;
using ChatClient.Commands.ContextMenuCommands;
using ChatClient.Enums;
using ChatClient.Models;
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
        public ChatViewModel(SignalRChatService chatService) : base(chatService)
        {
            //_getBan += GetBan_Action;

            InitializeFields(chatService);

            chatService.MessageReceived += ChatService_MessageReceived;
            chatService.UserListReceived += ChatService_UserListReceived;
            chatService.ReceivedBan += ChatService_ReceivedBan;
        }

        //private event Action _getBan;

        private string _message;
        private ObservableCollection<string> _activeUsers;
        private ObservableCollection<UserContextMenu> _contextMenuActions;

        public ObservableCollection<string> Messages { get; private set; }

        public ICommand SendChatMessageCommand { get; private set; }

        private void InitializeFields(SignalRChatService chatService)
        {
            SendChatMessageCommand = new SendChatCommand(this, chatService);
            Messages = new();

            _contextMenuActions = new ObservableCollection<UserContextMenu>()
            {
                new UserContextMenu()
                {
                    Header = "Ban",
                    Role = (int)UserRole.Admin,
                    Command = new BanUserCommand(chatService)
                }
            };
        }

        //private void GetBan_Action()
        //{
        //    OnPropertyChanged(nameof(IsBanned));
        //}

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

        private void ChatService_UserListReceived(ObservableCollection<string> activeUsers)
        {
            ActiveUsers = activeUsers;
        }
        private void ChatService_ReceivedBan(bool banResult)
        {
            UserStatusService.IsBanned = banResult;
            OnPropertyChanged(nameof(IsBanned));
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

        public ObservableCollection<UserContextMenu> ContextMenuActions
        {
            get => _contextMenuActions;
            set
            {
                _contextMenuActions = value;
                OnPropertyChanged();
            }
        }

        public bool IsBanned => UserStatusService.IsBanned;
    }
}
