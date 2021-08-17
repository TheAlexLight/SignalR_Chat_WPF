using ChatClient.Services;
using ChatClient.ViewModels;
using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Commands.ContextMenuCommands
{
    public class BanUserCommand : CommandBase
    {
        //private event Action _getBan;
        private readonly SignalRChatService _chatService;

        public BanUserCommand(SignalRChatService chatService/*Action getBan*/)
        {
            _chatService = chatService;
            //_getBan = getBan;
        }

        public override async void Execute(object activeUser)
        {
            UserProfileModel user = activeUser as UserProfileModel;

           await _chatService.SendBan(user.Username);
        }

        //public void ReceiveBan()
        //{
        //    _getBan?.Invoke();
        //}
    }
}
