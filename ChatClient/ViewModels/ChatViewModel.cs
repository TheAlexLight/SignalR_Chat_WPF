using ChatClient.Commands;
using ChatClient.Commands.AuthenticationCommands;
using ChatClient.Commands.ContextMenuCommands;
using ChatClient.Commands.ToolBarCommands;
using ChatClient.Enums;
using ChatClient.Interfaces;
using ChatClient.Models;
using ChatClient.Services;
using ChatClient.Stores;
using SharedItems.Models;
using SharedItems.Models.StatusModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChatClient.ViewModels
{
    public class ChatViewModel : ChatViewModelBase
    {
        public ChatViewModel(INavigator navigator, ISignalRChatService chatService) : base(navigator, chatService)
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
                window.MinWidth = 470;
                window.MinHeight = 350;
            }
        }

        //private event Action _getBan;

        private string _message;
        private ObservableCollection<MessageModel> _messages;
        private ObservableCollection<UserProfileModel> _activeUsers;
        private UserProfileModel _currentUser;

        public static readonly DependencyProperty TimeDurationProperty = DependencyProperty.RegisterAttached("DurationTime"
                , typeof(string), typeof(ChatViewModel), new PropertyMetadata(null));
        
        public static void SetDurationTime(DependencyObject element, string value)
        {
            element.SetValue(TimeDurationProperty, value);
        }

        public static string GetCustomValue(DependencyObject element)
        {
            return (string)element.GetValue(TimeDurationProperty);
        }

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

        public ICommand SendChatMessageCommand { get; private set; }
        public ICommand RemoveToolBarOverflowCommand { get; private set; }
        public ICommand KickUserCommand { get; private set; }
        public ICommand BanUserCommand { get; private set; }

        private void InitializeFields(ISignalRChatService chatService)
        {
            SendChatMessageCommand = new SendChatCommand(this, chatService);
            ReconnectionCommand = new ReconnectionCommand(this);
            BanUserCommand = new BanUserCommand(this);
            KickUserCommand = new KickUserCommand(this);
            RemoveToolBarOverflowCommand = new RemoveToolBarOverfowCommand();
            Messages = new();
        }
        private void InitializeEvents(ISignalRChatService chatService)
        {
            chatService.MessageReceived += ChatService_MessageReceived;
            chatService.UserListReceived += ChatService_UserListReceived;
            chatService.CurrentUserReceived += ChatService_CurrentUserReceived;
            chatService.SavedMessagesReceived += ChatService_SavedMessagesReceived;
            chatService.ReceivedBan += ChatService_ReceivedBan;
            chatService.ReceivedKick += ChatService_ReceivedKick;
        }

        public static ChatViewModel CreateConnectedViewModel(ISignalRChatService chatService, INavigator navigator)
        {
            ChatViewModel viewModel = new(navigator, chatService);

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

        private void ChatService_ReceivedBan(BanStatusModel model)
        {
            //UserStatusService.IsBanned = banResult;
            //OnPropertyChanged(nameof(IsBanned));
        }

        private async void ChatService_ReceivedKick()
        {
            await ChatService.Connection.StopAsync();

            NavigationService<LoginViewModel> navigationService = new(Navigator,
          () => new LoginViewModel(Navigator, ChatService));

            navigationService.Navigate();
            MessageBox.Show("You have been kicked.");
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
    }
}
