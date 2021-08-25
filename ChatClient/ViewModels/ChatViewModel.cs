using ChatClient.Commands;
using ChatClient.Commands.AuthenticationCommands;
using ChatClient.Commands.ContextMenuCommands;
using ChatClient.Enums;
using ChatClient.Models;
using ChatClient.Services;
using ChatClient.Stores;
using SharedItems.Models;
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

        public ChatViewModel(SignalRChatService chatService, NavigationStore navigationStore) : base(chatService, navigationStore)
        {
            //_getBan += GetBan_Action;

            InitializeFields(chatService);
            InitializeEvents(chatService);

            Window window = Application.Current.MainWindow;

            if (window != null)
            {
                window.Height = 400;
                window.Width = 550;
                window.Top = 110;
                window.Left = 415;
                window.MinWidth = 400;
                window.MinHeight = 350;
            }
        }

        //private event Action _getBan;

        private string _message;
        private ObservableCollection<MessageModel> _messages;
        private ObservableCollection<UserProfileModel> _activeUsers;
        private UserProfileModel _currentUser;

        public UserProfileModel CurrentUser {
            get => _currentUser;
            private set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<MessageModel> Messages
        {
            get => _messages;
            private set
            {
                _messages = value;
                OnPropertyChanged();
            }
        }
        //public ObservableCollection<UserContextMenu> ContextMenuActions { get; private set; }

        public ICommand SendChatMessageCommand { get; private set; }

        private void InitializeFields(SignalRChatService chatService)
        {
            SendChatMessageCommand = new SendChatCommand(this, chatService);
            ReconnectionCommand = new ReconnectionCommand(this);
            Messages = new();

            //Context = new ObservableCollection<UserContextMenu>()
            //{
            //    new UserContextMenu()
            //    {
            //        Header = "Ban",
            //        Role = (int)UserRole.Admin,
            //        Command = new BanUserCommand(chatService)
            //    }
            //};
        }
        private void InitializeEvents(SignalRChatService chatService)
        {
            chatService.MessageReceived += ChatService_MessageReceived;
            chatService.UserListReceived += ChatService_UserListReceived;
            chatService.ReceivedBan += ChatService_ReceivedBan;
            chatService.CurrentUserReceived += ChatService_CurrentUserReceived;
            chatService.SavedMessagesReceived += ChatService_SavedMessagesReceived;
        }

        //private void GetBan_Action()
        //{
        //    OnPropertyChanged(nameof(IsBanned));
        //}

        public static ChatViewModel CreateConnectedViewModel(SignalRChatService chatService, NavigationStore navigationStore)
        {
            ChatViewModel viewModel = new(chatService, navigationStore);

            chatService.Connect().ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    viewModel.ErrorMessage = "Unable to connect to chat hub";
                }
            });

            return viewModel;
        }

        protected async override Task Reconnect()
        {
            await ChatService.Reconnect(CurrentUser.Username);
        }

        private void ChatService_CurrentUserReceived(UserProfileModel currentUser)
        {
            CurrentUser = currentUser;
        }

        private void ChatService_UserListReceived(ObservableCollection<UserProfileModel> activeUsers)
        {
            ActiveUsers = activeUsers;
            OnPropertyChanged(nameof(ActiveUsers));
        }

        private void ChatService_SavedMessagesReceived(List<MessageModel> messages)
        {
            Messages = new ObservableCollection<MessageModel>(messages);
        }

        private void ChatService_MessageReceived(MessageModel message)
        {
            Messages.Add(message);
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

        public ObservableCollection<UserProfileModel> ActiveUsers
        {
            get => _activeUsers;
            private set
            {
                _activeUsers = value;
                OnPropertyChanged();
            }
        }

        public bool IsBanned => UserStatusService.IsBanned;
    }
}
