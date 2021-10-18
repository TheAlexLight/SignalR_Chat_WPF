using ChatClient.ViewModels;
using SharedItems.Enums;
using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChatClient.Commands
{
    public class ElementLoadedCommand : CommandBase
    {
        private readonly ChatViewModel _viewModel;

        public ElementLoadedCommand(ChatViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            if (parameter is MessageModel message)
            {
                if (message.CheckStatus == MessageStatus.Received)
                {
                    message.CheckStatus = MessageStatus.Read;
                    //var a = _viewModel.CurrentChatGroup.Messages.ToList()[0].CheckStatus;
                    //a = MessageStatus.Read;
                }
            }
        }
    }
}
