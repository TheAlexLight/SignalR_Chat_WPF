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
        public event Action<UserProfileModel> CurrentUserReceived;
        public event Action<ObservableCollection<UserProfileModel>> UserListReceived;
        public event Action<List<MessageModel>> SavedMessagesReceived;
        public event Action<MessageModel> MessageReceived;
        public event Action<BanStatusModel> ReceivedBan;
        public event Action ReceivedKick;
        public event Action<MuteStatusModel> ReceivedMute;

        public Task Connect();
        public Task Reconnect(string username);
        public Task SendMessage(MessageModel message);
        public Task Registration(UserRegistrationModel model);
        public Task Login(UserLoginModel model);
        public Task SendBan(string username, BanStatusModel model);
        public Task SendMute(string username, MuteStatusModel model);
        public Task KickUser(string username);
    }
}
