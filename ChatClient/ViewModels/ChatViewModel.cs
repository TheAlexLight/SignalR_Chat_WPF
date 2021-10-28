using ChatClient.Commands;
using ChatClient.Commands.AuthenticationCommands;
using ChatClient.Commands.ChangeUserPropertyCommand;
using ChatClient.Commands.ContextMenuCommands;
using ChatClient.Commands.CustomViewsCommands;
using ChatClient.Commands.ToolBarCommands;
using ChatClient.Enums;
using ChatClient.Interfaces;
using ChatClient.Services;
using ChatClient.Stores;
using Newtonsoft.Json;
using SharedItems.Enums;
using SharedItems.Models;
using SharedItems.Models.StatusModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace ChatClient.ViewModels
{
    public class ChatViewModel : ChatViewModelBase, IDataErrorInfo
    {
        public ChatViewModel(INavigator navigator, ISignalRChatService chatService
               , IWindowConfigurationService windowConfigurationService) : base(navigator, chatService, windowConfigurationService)
        {

            InitializeFields(chatService);
            InitializeEvents(chatService);

            Window window = Application.Current.MainWindow;

            if (window != null)
            {
                windowConfigurationService.SetWindowStartupData(
                    window: window,
                    left: 415,
                    top: 110,
                    width: 550,
                    height: 415,
                    minWidth: 540,
                    minHeight: 350);
            }
        }

        private string _message;
        private string _usersFilter;
        private string _usernameSettingsText;
        private string _emailSettingsText;
        //private ObservableCollection<MessageModel> _messages;
        private ObservableCollection<UserModel> _allUsers;
        private ObservableCollection<Group> _groups;
        private UserModel _currentUser; 
        private UserModel _selectedUser; 
        private MuteStatusModel _muteStatus;
        private ChatGroupModel _currentChatGroup;
        private int _selectedUserIndex;
        private bool _isUserInfoOpened;
        private bool _isSettingsOpened;
        private bool _needToClearPassword;
        private string _password;
        private string _passwordConfirm;
        private GridLength _usersColumnWidth;
        private GridLength _messagesColumnWidth;

        private ChatType _currentChatType;
        private ChangeSettingsType _userSettingsType;

        public ICollectionView UsersCollectionView { get; private set; }
        public ICollectionView FilterUsersCollectionView { get; private set; }

        public ICommand SendChatMessageCommand { get; private set; }
        public ICommand RemoveToolBarOverflowCommand { get; private set; }
        public ICommand KickUserCommand { get; private set; }
        public ICommand BanUserCommand { get; private set; }
        public ICommand MuteUserCommand { get; private set; }
        public ICommand SwitchChatCommand { get; private set; }
        public ICommand UpdatePrivateMessagesCommand { get; private set; }
        public ICommand OpenUserInfoWIndowCommand { get; private set; }
        public ICommand OpenSettingsCommand { get; private set; }
        public ICommand ChangePhotoCommand { get; private set; }
        public ICommand MessageReadCommand { get; private set; }
        public ICommand MessageLoadedCommand { get; private set; }
        public ICommand ChangeUsernameCommand { get; private set; }
        public ICommand ChangeEmailCommand { get; private set; }
        public ICommand ChangePasswordCommand { get; private set; }
        public ICommand ChangeUserSettingsCommand { get; private set; }



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

        private void InitializeFields(ISignalRChatService chatService)
        {
            SendChatMessageCommand = new SendChatCommand(this, chatService);
            ReconnectionCommand = new ReconnectionCommand(this, CurrentUser);
            BanUserCommand = new BanUserCommand(this);
            KickUserCommand = new KickUserCommand(this);
            MuteUserCommand = new MuteUserCommand(this);
            SwitchChatCommand = new SwitchChatCommand(this);
            RemoveToolBarOverflowCommand = new RemoveToolBarOverfowCommand();
            UpdatePrivateMessagesCommand = new UpdatePrivateMessagesCommand(this);
            OpenUserInfoWIndowCommand = new OpenUserInfoWIndowCommand(this);
            OpenSettingsCommand = new OpenSettingsCommand(this);
            ChangePhotoCommand = new ChangePhotoCommand(this);
            MessageReadCommand = new MessageReadCommand();
            MessageLoadedCommand = new MessageLoadedCommand();
            ChangeUserSettingsCommand = new ChangeUserSettingsCommand(this);
            ChangeUsernameCommand = new ChangeUsernameCommand(this);
            ChangePasswordCommand = new ChangePasswordCommand(this);
            ChangeEmailCommand = new ChangeEmailCommand(this);
            //Messages = new();
            MuteStatus = new();
            UsersFilter = string.Empty;
            AllUsers = new ObservableCollection<UserModel>();
            UsersColumnWidth = new GridLength(1, GridUnitType.Star);
            MessagesColumnWidth = new GridLength(0.7, GridUnitType.Star);

            CreateGroups();

            UsersCollectionView = CollectionViewSource.GetDefaultView(Groups);
            UsersCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Group.Name)));
        }

        private void InitializeEvents(ISignalRChatService chatService)
        {
            //chatService.MessageReceived += ChatService_MessageReceived;
            chatService.UserListReceived += ChatService_UserListReceived;
            chatService.CurrentGroupReceived += ChatService_CurrentGroupReceived;
            chatService.CurrentUserReceived += ChatService_CurrentUserReceived;
            //chatService.CurrentGroupReceived += ChatService_SavedMessagesReceived;
            chatService.ReceivedBan += ChatService_ReceivedBan;
            chatService.ReceivedKick += ChatService_ReceivedKick;
            chatService.ReceivedMute += ChatService_ReceivedMute;
            chatService.ReceivedUserPropertyChange += ChatService_ReceivedUserPropertyChange;
        }

        private bool FilterUsers(object obj)
        {
            bool result = false;

            if (obj is UserModel user)
            {
                result = user.UserProfile.Username.Contains(UsersFilter, StringComparison.InvariantCultureIgnoreCase);
            }

            return result;
        }

        private void CreateGroups()
        {
            Groups = new ObservableCollection<Group>()
            {
                new Group()
                {
                    Name = nameof(UserGroups.Online),
                },
                new Group()
                {
                    Name = nameof(UserGroups.Offline)
                },
                new Group()
                {
                    Name = nameof(UserGroups.Banned)
                }
            };
        }

        public static ChatViewModel CreateConnectedViewModel(ISignalRChatService chatService, INavigator navigator
                , IWindowConfigurationService windowConfiguration)
        {
            ChatViewModel viewModel = new(navigator, chatService, windowConfiguration);

            chatService.Connect().ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    viewModel.ErrorMessage = "Unable to connect to chat hub";
                }
            });

            return viewModel;
        }

        public string Error { get; }

        public string this[string propertyName]
        {
            get
            {
                string result = string.Empty;

                propertyName ??= string.Empty;

                if (propertyName != string.Empty
                        && (propertyName == nameof(PasswordConfirm) || propertyName == nameof(Password))
                        && PasswordConfirm != null && Password != null)
                {
                    if (!Password.Equals(PasswordConfirm, StringComparison.Ordinal))
                    {
                        result = "Passwords do not match";
                    }
                }

                return result;
            }
        }

        protected async override Task Reconnect()
        {
            await ChatService.Reconnect(CurrentUser.UserProfile.Username);
        }

        private void ChatService_CurrentUserReceived(UserModel currentUser)
        {
            CurrentUser = currentUser;
        }

        private void ChatService_CurrentGroupReceived(string currentGroup)
        {
            ChatGroupModel group = JsonConvert.DeserializeObject<ChatGroupModel>(currentGroup);
            CurrentChatGroup = group;

            Group bannedGroup = Groups.FirstOrDefault(g => g.Name.Equals(nameof(UserGroups.Banned)));
            bannedGroup.GroupedUsers = new ObservableCollection<UserModel>(CurrentChatGroup.Users
                    .Where(u => u.UserStatus.BanStatus.IsBanned).ToList());

            Group onlineGroup = Groups.FirstOrDefault(g => g.Name.Equals(nameof(UserGroups.Online)));
            onlineGroup.GroupedUsers = new ObservableCollection<UserModel>(CurrentChatGroup.Users
                    .Where(u => u.UserProfile.IsOnline).Except(bannedGroup.GroupedUsers).ToList());

            Group offlineGroup = Groups.FirstOrDefault(g => g.Name.Equals(nameof(UserGroups.Offline)));
            offlineGroup.GroupedUsers = new ObservableCollection<UserModel>(CurrentChatGroup.Users
                    .Except(bannedGroup.GroupedUsers).Except(onlineGroup.GroupedUsers).ToList());

            try
            {
                UsersCollectionView.Refresh();
            }
            catch (Exception)
            {            }
            
        }

        private void ChatService_UserListReceived(ObservableCollection<UserModel> allUsers)
        {
            AllUsers = allUsers;

            if (FilterUsersCollectionView == null)
            {
                FilterUsersCollectionView = CollectionViewSource.GetDefaultView(AllUsers);
                FilterUsersCollectionView.Filter = FilterUsers;
            }

            try
            {
                FilterUsersCollectionView.Refresh();
            }
            catch
            {}
        }

        private async void ChatService_ReceivedBan(BanStatusModel model)
        {
            await ChatService.Connection.StopAsync();

            NavigationService<BanViewModel> navigationService = new(Navigator,
                () => new BanViewModel(Navigator, ChatService, WindowConfigurationService, model, CurrentUser));

            navigationService.Navigate();
        }

        private void ChatService_ReceivedMute(MuteStatusModel model)
        {
            MuteStatus.IsMuted = model.IsMuted;
        }

        private async void ChatService_ReceivedKick()
        {
            await ChatService.Connection.StopAsync();

            NavigationService<LoginViewModel> navigationService = new(Navigator,
          () => new LoginViewModel(Navigator, ChatService, WindowConfigurationService));

            navigationService.Navigate();
            MessageBox.Show("You have been kicked.");
        }

        private Task ChatService_ReceivedUserPropertyChange(bool success, string PropertyName, string message)
        {
            if (success)
            {
                Task.Run(() =>
                {
                    MessageBox.Show(string.Format("{0} was changed successfully", PropertyName));
                });
                
                ChangeUserSettingsCommand.Execute(ChangeSettingsType.None);
            }
            else
            {
                MessageBox.Show(message);
            }

            return Task.CompletedTask;
        }

        public UserModel CurrentUser
        {
            get => _currentUser;
            private set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        public UserModel SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
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

        public ObservableCollection<UserModel> AllUsers
        {
            get => _allUsers;
            private set
            {
                _allUsers = value;
                OnPropertyChanged();
                //if (FilterUsersCollectionView != null)
                //{
                //    FilterUsersCollectionView.Refresh();
                //}
            }
        }

        public MuteStatusModel MuteStatus
        {
            get => _muteStatus;
            private set
            {
                _muteStatus = value;
                OnPropertyChanged();
            }
        }

        public ChatType CurrentChatType
        {
            get => _currentChatType;
            set
            {
                _currentChatType = value;
                OnPropertyChanged();
            }
        }

        public ChatGroupModel CurrentChatGroup
        {
            get => _currentChatGroup;
            set
            {
                _currentChatGroup = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Group> Groups
        {
            get => _groups;
            set
            {
                _groups = value;
                OnPropertyChanged();

                if (UsersCollectionView != null)
                {
                    UsersCollectionView.Refresh();
                }
            }
        }

        public string UsersFilter
        {
            get => _usersFilter;
            set
            {
                _usersFilter = value;
                OnPropertyChanged();

                if (FilterUsersCollectionView != null)
                {
                    FilterUsersCollectionView.Refresh();
                }
            }
        }

        public bool IsUserInfoOpened
        {
            get => _isUserInfoOpened;
            set
            {
                _isUserInfoOpened = value;
                OnPropertyChanged();
            }
        }

        public GridLength UsersColumnWidth
        {
            get => _usersColumnWidth;
            set
            {
                _usersColumnWidth = value;
                OnPropertyChanged();
            }
        }

        public GridLength MessagesColumnWidth
        {
            get => _messagesColumnWidth;
            set
            {
                _messagesColumnWidth = value;
                OnPropertyChanged();
            }
        }

        public bool IsSettingsOpened
        {
            get => _isSettingsOpened;
            set
            {
                _isSettingsOpened = value;
                OnPropertyChanged();
            }
        }

        public bool NeedToClearPassword
        {
            get => _needToClearPassword;
            set
            {
                _needToClearPassword = value;
                OnPropertyChanged();
            }
        }

        public string UsernameSettingsField
        {
            get => _usernameSettingsText;
            set
            {
                _usernameSettingsText = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string PasswordConfirm
        {
            get => _passwordConfirm;
            set
            {
                _passwordConfirm = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Password));
            }
        }

        public string EmailSettingsField
        {
            get => _emailSettingsText;
            set
            {
                _emailSettingsText = value;
                OnPropertyChanged();
            }
        }

        public ChangeSettingsType UserSettingsType
        {
            get => _userSettingsType;
            set
            {
                _userSettingsType = value;
                OnPropertyChanged();
            }
        }

        public int SelectedUserIndex
        {
            get => _selectedUserIndex;
            set
            {
                    Window window = Application.Current.MainWindow;

                    if (value == -1)
                    {
                    //window.MinWidth = 260;
                    //window.Width = 260;
                    }
                    else
                    {
                    //window.MinWidth = 470;
                    //window.Width = 550;
                    }

                _selectedUserIndex = value;
                OnPropertyChanged();
            }
        }
    }
}
