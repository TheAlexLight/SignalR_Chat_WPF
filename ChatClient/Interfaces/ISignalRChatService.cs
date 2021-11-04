using SharedItems.Enums;
using SharedItems.Models;
using SharedItems.Models.AuthenticationModels;
using SharedItems.Models.StatusModels;
using SharedItems.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Interfaces
{
    public interface ISignalRChatService : ISignalRService
    {
        public event Action<bool, string> ReceiveRegistrationResult;
        public event Action<bool> ReceiveLoginResult;
        public event Action<UserModel> CurrentUserReceived;
        public event Action<List<UserModel>> UserListReceived;
        public event Action<ChatGroupModel> CurrentGroupReceived;
        //public event Action<MessageModel> MessageReceived;
        public event Action<BanStatusModel> ReceivedBan;
        public event Action ReceivedKick;
        public event Action<MuteStatusModel> ReceivedMute;
        public event Func<bool, string, string, Task> ReceivedUserPropertyChange;

        public Task Connect();
        public Task Reconnect(string username);
        public Task SendMessage(MessageModel message, ChatGroupModel currentGroup, UserModel selectedUsr, UserModel currentUser);
        public Task UpdateMessage(MessageModel newMessage, ChatGroupModel currentGroup);
        public Task Registration(UserRegistrationModel model);
        public Task Login(UserLoginModel model);
        public Task SendBan(string username, BanStatusModel model);
        public Task SendMute(string username, MuteStatusModel model);
        public Task KickUser(string username);
        public Task SwitchChat(ChatType chatType);
        public Task UpdatePrivateMessages(UserModel selectedUser, UserModel currentUser);
        public Task UpdateMessage(MessageModel message);
        public Task ChangePhoto(UserModel currentUser, byte[] photo);
        //public Task SendImage(UserModel currentUser, byte[] image);
        public Task SubmitUsernameChange(UserModel user, string username, string password);
        public Task SubmitEmailChange(UserModel user, string username, string password);
        public Task SubmitPasswordChange(UserModel user, string username, string password);
    }
}
