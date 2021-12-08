using ChatClient.MVVM.ViewModels.ChatFeaturesModels;
using ChatClient.Services.BaseConfiguration;
using ChatClient.Services.ConcreteConfiguration;
using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Commands.ContextMenuCommands
{
    public class DeleteMessageCommand : CommandBase
    {
        private readonly SignalRChatService _chatService;

        public DeleteMessageCommand(SignalRChatService chatService)
        {
            _chatService = chatService;
        }

        public override void Execute(object parameter)
        {
            if (parameter is MessageViewModel messageModel)
            {
                _chatService.MessageContextMenu.Delete(messageModel.MessageModel);
            }
        }
    }
}
