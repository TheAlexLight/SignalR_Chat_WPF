using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Interfaces.SignalRTransmitting.SignalTranmittingResult
{
    public interface IUsersUpdateResult
    {
        public event Action<UserModel> CurrentUserReceived;
        public event Action<ObservableCollection<UserModel>> UserListReceived;
        public event Action<ChatGroupModel> CurrentGroupReceived;
    }
}
