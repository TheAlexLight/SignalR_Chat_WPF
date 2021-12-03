using ChatClient.Interfaces.SignalRTransmitting;
using Microsoft.AspNetCore.SignalR.Client;
using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.MVVM.Models.SignalRModels
{
    public class MessagesManagementModel : IMessagesManagement
    {
        private readonly IEventHandler _chatService;

        public MessagesManagementModel(IEventHandler chatService)
        {
            _chatService = chatService;
        }

        public async Task SendMessage(MessageModel message, ChatGroupModel currentGroup, UserModel selectedUser, UserModel currentUser)
        {
            await _chatService.Connection.SendAsync("SendMessage", message, currentGroup, selectedUser, currentUser);
        }

        public async Task UpdateMessage(MessageModel message, ChatGroupModel currentGroup)
        {
            await _chatService.Connection.SendAsync("SendUpdateMessage", message, currentGroup);
        }

        public async Task UpdatePrivateMessages(UserModel selectedUser, UserModel currentUser)
        {
            await _chatService.Connection.SendAsync("SendUpdatePrivateMessages", selectedUser, currentUser);
        }
    }
}
