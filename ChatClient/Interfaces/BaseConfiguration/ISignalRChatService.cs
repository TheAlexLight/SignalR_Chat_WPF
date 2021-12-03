using ChatClient.Interfaces.SignalRTransmitting;
using ChatClient.Interfaces.SignalRTransmitting.SignalTranmittingResult;
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

namespace ChatClient.Interfaces.BaseConfiguration
{
    public interface ISignalRChatService : IEventHandler//, ICredentialResult
    //    ,IAdminActionResult, IMessagesManagement, IUsersUpdateResult
    {
        public Task SwitchChat(ChatType chatType, UserModel currentUser);
        public Task ChangePhoto(UserModel currentUser, byte[] photo);
    }
}
