using ChatClient.Commands;
using ChatClient.Commands.ContextMenuCommands;
using ChatClient.MVVM.ViewModels.ChatMainViewModels;
using ChatClient.Services.ConcreteConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatClient.MVVM.Models.CommandModels
{
    public class MessageCommandModel
    {
        public MessageCommandModel(ChatViewModel viewModel, SignalRChatService chatService)
        {
            MessageReadCommand = new MessageReadCommand(viewModel);

            SendMessageCommand = new SendMessageCommand(viewModel, chatService);

            UpdatePrivateMessagesCommand = new UpdatePrivateMessagesCommand(viewModel);

            DeleteMessageCommand = new DeleteMessageCommand(chatService);
        }

        public ICommand MessageReadCommand { get;}
        public ICommand SendMessageCommand { get;}
        public ICommand UpdatePrivateMessagesCommand { get;}
        public ICommand DeleteMessageCommand { get;}
    }
}
