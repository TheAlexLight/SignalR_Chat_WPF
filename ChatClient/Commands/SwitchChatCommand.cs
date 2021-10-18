using ChatClient.Enums;
using ChatClient.ViewModels;
using SharedItems.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.Commands
{
    public class SwitchChatCommand : CommandBase
    {
        private readonly ChatViewModel _viewModel;

        public SwitchChatCommand(ChatViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            if (parameter is ChatType currentChatType)
            {
                _viewModel.CurrentChatType = currentChatType;
                _viewModel.Message = string.Empty;

                if (currentChatType == ChatType.Private)
                {
                    _viewModel.UsersColumnWidth = new GridLength(0.75, GridUnitType.Star);
                    _viewModel.MessagesColumnWidth = new GridLength(2.5, GridUnitType.Star);
                    _viewModel.SelectedUserIndex = -1;
                }
                else
                {
                    _viewModel.UsersColumnWidth = new GridLength(1, GridUnitType.Star);
                    _viewModel.MessagesColumnWidth = new GridLength(0.7, GridUnitType.Star);
                    _viewModel.SelectedUserIndex = 0;
                }

                _viewModel.ChatService.SwitchChat(currentChatType);

                Thread.Sleep(100);
            }
        }
    }
}
