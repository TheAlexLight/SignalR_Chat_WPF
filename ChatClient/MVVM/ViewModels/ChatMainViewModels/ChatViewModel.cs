using ChatClient.Commands.AuthenticationCommands;
using ChatClient.Enums;
using ChatClient.Interfaces;
using ChatClient.MVVM.Models.CommandModels.CommandModelsBase;
using ChatClient.MVVM.ViewModels.BaseViewModels;
using ChatClient.MVVM.ViewModels.ChatFeaturesModels;
using ChatClient.Services.BaseConfiguration;
using ChatClient.Services.ConcreteConfiguration;
using SharedItems.Enums;
using SharedItems.Models;
using SharedItems.Models.StatusModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChatClient.MVVM.ViewModels.ChatMainViewModels
{
    public class ChatViewModel : ChatViewModelBase, IDataErrorInfo, IFilesDropped
    {
        public ChatViewModel(ChatBaseConfiguration baseConfiguration) : base(baseConfiguration)
        {
            CommandsModel = new ChatCommandsModelBase(this, baseConfiguration.ChatService);
            ReconnectionCommand = new ReconnectionCommand(this, CurrentUser);

            InitializeFields();
            InitializeEvents(BaseConfiguration.ChatService);

            Window window = Application.Current.MainWindow;

            if (window != null)
            {
                BaseConfiguration.WindowConfigurationService.SetWindowStartupData(
                    window: window,
                    left: 415,
                    top: 110,
                    width: 550,
                    height: 415,
                    minWidth: 540,
                    minHeight: 350);
            }
        }

        #region Private Fields
        private string _usersFilter;
        private string _usernameSettingsText;
        private string _emailSettingsText;
        private string _resetScroll;
        private string _password;
        private string _passwordConfirm;
        private string _descriptionText;
        //private ObservableCollection<MessageModel> _messages;

        private MessageInformationModel _message;
        private ObservableCollection<UserModel> _allUsers;
        private ObservableCollection<Group> _groups;
        private UserModel _currentUser;
        private UserModel _selectedUser;
        private MuteStatusModel _muteStatus;
        private ChatGroupViewModel _currentChatGroup;
        private UserModel _temporarySelectedItem = null;

        private object _selectedItem;

        private int _selectedUserIndex;
        //private int _temporarySelectedIndex = -1;

        private bool _isUserInfoOpened;
        private bool _isSettingsOpened;
        private bool _needToClearPassword;
        private bool _needToUpdateMessagesCount;
        private bool _canCloseChat = true;

        private BitmapImage _descriptionImage;
        private GridLength _usersColumnWidth;
        private GridLength _messagesColumnWidth;

        private ChatType _currentChatType;
        private ChangeSettingsType _userSettingsType;

        private ICollectionView _usersCollectionView;
        private ICollectionView _filterUsersCollectionView;
        #endregion

        public ChatCommandsModelBase CommandsModel { get; set; }

        public ICollectionView UsersCollectionView
        {
            get => _usersCollectionView;
            set
            {
                _usersCollectionView = value;
                OnPropertyChanged();
            }
        }
        public ICollectionView FilterUsersCollectionView
        {
            get => _filterUsersCollectionView;
            set
            {
                _filterUsersCollectionView = value;
                OnPropertyChanged();
            }
        }

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

        private void InitializeFields()
        {
            Message = new();
            MuteStatus = new();
            UsersFilter = string.Empty;
            AllUsers = new ObservableCollection<UserModel>();
            CurrentChatGroup = new();
            UsersColumnWidth = new GridLength(1, GridUnitType.Star);
            MessagesColumnWidth = new GridLength(2.5, GridUnitType.Star);

            CreateGroups();

            UsersCollectionView = CollectionViewSource.GetDefaultView(Groups);
            UsersCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Group.Name)));
            UsersCollectionView.Filter = UsersGroupFilter;

            FilterUsersCollectionView = CollectionViewSource.GetDefaultView(AllUsers);
            FilterUsersCollectionView.Filter = FilterUsers;
        }

        private bool UsersGroupFilter(object obj)
        {
            //Group group = obj as Group;

            //ICollectionView usersCollectionView = CollectionViewSource.GetDefaultView(group.GroupedUsers);

            //if (usersCollectionView != null)
            //{
            //    usersCollectionView.Filter = user => ((UserModel)user).UserProfile.Username != CurrentUser.UserProfile.Username;

            //    //return group.GroupedUsers.All(user => user.UserProfile.Username != CurrentUser.UserProfile.Username);
            //}

            return true;
            //UsersCollectionView.GetDefaultView(group.GroupedUsers);


            //bool result = false;



            //if (obj is Group group && group.GroupedUsers != null)
            //{
            //    result = group.GroupedUsers.Any(u => u.UserProfile.Username != "User1");
            //}

            //return result;
        }

        private void InitializeEvents(SignalRChatService chatService)
        {
            chatService.UsersUpdateModel.CurrentUserReceived += ChatService_CurrentUserReceived;
            chatService.UsersUpdateModel.UserListReceived += ChatService_UserListReceived;
            chatService.UsersUpdateModel.CurrentGroupReceived += ChatService_CurrentGroupReceived;

            chatService.AdminActionModel.BanResultReceived += ChatService_ReceivedBan;
            chatService.AdminActionModel.KickResultReceived += ChatService_ReceivedKick;
            chatService.AdminActionModel.MuteResultReceived += ChatService_ReceivedMute;

            chatService.CredentialModel.UserCredentialsResultReceived += ChatService_UserCredentialsResultReceived;
        }

        private bool FilterUsers(object obj)
        {
            bool result = false;

            if (obj is UserModel user)
            {
                result = user.UserProfile.Username.Contains(UsersFilter, StringComparison.InvariantCultureIgnoreCase)
                    && user.UserProfile.Username != CurrentUser.UserProfile.Username;
            }

            return result;

            //turn true;
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

        public static ChatViewModel CreateConnectedViewModel(ChatBaseConfiguration baseConfiguration)
        {
            ChatViewModel viewModel = new(baseConfiguration);

            baseConfiguration.ChatService.AuthorizationModel.Connect().ContinueWith(task =>
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
            await BaseConfiguration.ChatService.AuthorizationModel.Reconnect(CurrentUser.UserProfile.Username);
        }

        private void ChatService_CurrentUserReceived(UserModel currentUser)
        {
            CurrentUser = currentUser;
        }

        private void ChatService_CurrentGroupReceived(ChatGroupModel currentGroup)
        {
            if (CurrentChatType == currentGroup.Name)
            {
                if (CurrentChatType == ChatType.Public)
                {
                    currentGroup.Users = currentGroup.Users.Where(um => um.UserProfile.Username != CurrentUser.UserProfile.Username).ToList();
                }

                if (CurrentChatType == ChatType.Public || CurrentChatGroup.CurrentChatGroupModel == null || (SelectedItem != null && currentGroup.Users
                        .Any(u=>u.UserProfile.Username == ((UserModel)SelectedItem).UserProfile.Username)))
                {
                    CurrentChatGroup.CurrentChatGroupModel = currentGroup;

                    CurrentChatGroup.MessagesViewModel.Clear();
                    
                    foreach (MessageModel messageModel in CurrentChatGroup.CurrentChatGroupModel.Messages)
                    {
                        CurrentChatGroup.MessagesViewModel.Add(new MessageViewModel(messageModel));
                    }
                }

                Group bannedGroup = Groups.FirstOrDefault(g => g.Name.Equals(nameof(UserGroups.Banned)));
                bannedGroup.GroupedUsers = new ObservableCollection<UserModel>(CurrentChatGroup.CurrentChatGroupModel.Users
                        .Where(u => u.UserStatus.BanStatus.IsBanned).ToList());

                bannedGroup.UsersCount = bannedGroup.GroupedUsers.Where(user => user.UserProfile.Username != CurrentUser.UserProfile.Username).Count();

                Group onlineGroup = Groups.FirstOrDefault(g => g.Name.Equals(nameof(UserGroups.Online)));
                onlineGroup.GroupedUsers = new ObservableCollection<UserModel>(CurrentChatGroup.CurrentChatGroupModel.Users
                        .Where(u => u.UserProfile.IsOnline).Except(bannedGroup.GroupedUsers).ToList());

                onlineGroup.UsersCount = onlineGroup.GroupedUsers.Where(user => user.UserProfile.Username != CurrentUser.UserProfile.Username).Count();

                Group offlineGroup = Groups.FirstOrDefault(g => g.Name.Equals(nameof(UserGroups.Offline)));
                offlineGroup.GroupedUsers = new ObservableCollection<UserModel>(CurrentChatGroup.CurrentChatGroupModel.Users
                        .Except(bannedGroup.GroupedUsers).Except(onlineGroup.GroupedUsers).ToList());

                offlineGroup.UsersCount = offlineGroup.GroupedUsers.Where(user => user.UserProfile.Username != CurrentUser.UserProfile.Username).Count();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    UsersCollectionView.Refresh();
                });
            }


        }

        private void ChatService_UserListReceived(ObservableCollection<UserModel> allUsers)
        {
            //if (AllUsers.Count != 0 && SelectedUserIndex != -1)
            //{
            //    _temporarySelectedItem = (UserModel)SelectedItem;

            //    _canCloseChat = false;
            //    //_temporarySelectedIndex = SelectedUserIndex;
            //}

            _canCloseChat = false;

            List<UserModel> allUsersExceptCurrent = allUsers.Where(u => u.UserProfile.Username != CurrentUser.UserProfile.Username).ToList();

            AllUsers = new ObservableCollection<UserModel>(allUsersExceptCurrent);

            FilterUsersCollectionView = CollectionViewSource.GetDefaultView(AllUsers);
            FilterUsersCollectionView.Filter = FilterUsers;

            //SelectedItem = _temporarySelectedItem;

            //if (_temporarySelectedItem != null)
            //{
            //    SelectedUserIndex = AllUsers.IndexOf(AllUsers.First(u => u.UserProfile.Id == _temporarySelectedItem.UserProfile.Id));
            //}

            _canCloseChat = true;
        }

        private async void ChatService_ReceivedBan(BanStatusModel model)
        {
            await BaseConfiguration.ChatService.Connection.StopAsync();

            BaseConfiguration.CreateConcreteNavigationService<BanViewModel>(BaseConfiguration, model, CurrentUser);

            BaseConfiguration.NavigationService.Navigate();
        }

        private void ChatService_ReceivedMute(MuteStatusModel model)
        {
            MuteStatus.IsMuted = model.IsMuted;
        }

        private async void ChatService_ReceivedKick()
        {
            await BaseConfiguration.ChatService.Connection.StopAsync();

            BaseConfiguration.CreateConcreteNavigationService<LoginViewModel>(BaseConfiguration);

            BaseConfiguration.NavigationService.Navigate();

            MessageBox.Show("You have been kicked.");
        }

        private Task ChatService_UserCredentialsResultReceived(bool success, string PropertyName, string message)
        {
            if (success)
            {
                Task.Run(() =>
                {
                    MessageBox.Show(string.Format("{0} was changed successfully", PropertyName));
                });

              CommandsModel.UserCredentialsCommandModel.ChangeUserSettingsCommand.Execute(ChangeSettingsType.None);
            }
            else
            {
                MessageBox.Show(message);
            }

            return Task.CompletedTask;
        }

        public void OnFilesDropped(string[] files)
        {
            foreach (string image in files)
            {
                string imageExtension = Path.GetExtension(image).ToUpperInvariant();

                if (imageExtension == ".JPG"
                        || imageExtension == ".JPEG"
                        || imageExtension == ".PNG"
                        || imageExtension == ".BMP")
                {
                    object[] parameters = new object[3];
                    parameters[0] = MessageInformationType.Image;
                    parameters[2] = image;

                    if (CurrentChatType == ChatType.Private)
                    {
                        parameters[1] = (UserModel)SelectedItem;
                    }

                  CommandsModel.MessageCommandModel.SendMessageCommand.Execute(parameters);
                }
            }
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

        public MessageInformationModel Message
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
            set
            {
                _allUsers = value;
                OnPropertyChanged();
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

        public ChatGroupViewModel CurrentChatGroup
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
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        FilterUsersCollectionView.Refresh();
                    });
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

        public bool NeedToUpdateMessagesCount
        {
            get => _needToUpdateMessagesCount;
            set
            {
                _needToUpdateMessagesCount = value;
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

        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_canCloseChat)
                {
                    _selectedItem = value;
                }

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

        public string DescriptionText
        {
            get => _descriptionText;
            set
            {
                _descriptionText = value;
                OnPropertyChanged();
            }
        }

        public BitmapImage DescriptionImage
        {
            get => _descriptionImage;
            set
            {
                _descriptionImage = value;
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

        public string ResetScroll
        {
            get => _resetScroll;
            set
            {
                _resetScroll = value;
                OnPropertyChanged();
            }
        }

        public int SelectedUserIndex
        {
            get => _selectedUserIndex;
            set
            {
                if (_canCloseChat)
                {
                    _selectedUserIndex = value;
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(AllUsers));
            }
        }
    }
}