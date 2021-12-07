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
    public class MessageContextMenu : IContextMenu
    {
        private readonly IEventHandler _chatService;

        public MessageContextMenu(IEventHandler chatService)
        {
            _chatService = chatService;
        }

        public async Task Delete(MessageModel message)
        {
            await _chatService.Connection.SendAsync("SendDeleteMessage", message);
        }
    }
}
