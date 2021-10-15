using SharedItems.Enums;
using SharedItems.Models;
using SharedItems.Models.AuthenticationModels;
using SharedItems.Models.StatusModels;
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
        public event Action<ObservableCollection<UserModel>> UserListReceived;
        public event Action<string> CurrentGroupReceived;
        //public event Action<MessageModel> MessageReceived;
        public event Action<BanStatusModel> ReceivedBan;
        public event Action ReceivedKick;
        public event Action<MuteStatusModel> ReceivedMute;

        public Task Connect();
        public Task Reconnect(string username);
        public Task SendMessage(MessageModel message, ChatGroupModel currentGroup, UserModel selectedUsr, UserModel currentUser);
        public Task Registration(UserRegistrationModel model);
        public Task Login(UserLoginModel model);
        public Task SendBan(string username, BanStatusModel model);
        public Task SendMute(string username, MuteStatusModel model);
        public Task KickUser(string username);
        public Task SwitchChat(ChatType chatType);
        public Task UpdatePrivateMessages(UserModel selectedUser, UserModel currentUser);
        public Task ChangePhoto(UserModel currentUser, byte[] photo);
    }
}
