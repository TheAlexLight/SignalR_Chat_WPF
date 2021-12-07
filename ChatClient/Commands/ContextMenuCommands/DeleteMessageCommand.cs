using ChatClient.MVVM.ViewModels.ChatFeaturesModels;
using ChatClient.Services.BaseConfiguration;
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
        private readonly ChatBaseConfiguration _baseConfiguration;

        public DeleteMessageCommand(ChatBaseConfiguration baseConfiguration)
        {
            _baseConfiguration = baseConfiguration;
        }

        public override void Execute(object parameter)
        {
            if (parameter is MessageViewModel messageModel)
            {
                _baseConfiguration.ChatService.MessageContextMenu.Delete(messageModel.MessageModel);
            }
        }
    }
}
