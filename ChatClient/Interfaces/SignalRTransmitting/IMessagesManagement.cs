using SharedItems.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Interfaces.SignalRTransmitting
{
    public interface IMessagesManagement
    {
        public Task SendMessage(MessageModel message, ChatGroupModel currentGroup, UserModel selectedUsr, UserModel currentUser);
        public Task UpdateMessage(MessageModel newMessage, ChatGroupModel currentGroup);
        public Task UpdatePrivateMessages(UserModel selectedUser, UserModel currentUser);
    }
}
