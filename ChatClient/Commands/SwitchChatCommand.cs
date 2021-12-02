using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using ChatClient.MVVM.ViewModels.ChatMainViewModels;
using SharedItems.Enums;

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
                _viewModel.Message.TextMessage = string.Empty;
                _viewModel.CanCloseChat = true;

                if (currentChatType == ChatType.Private)
                {
                    _viewModel.UsersColumnWidth = new GridLength(1, GridUnitType.Star);
                    _viewModel.MessagesColumnWidth = new GridLength(2.5, GridUnitType.Star);
                    _viewModel.SelectedUserIndex = -1;
                }
                else
                {
                    _viewModel.UsersColumnWidth = new GridLength(1, GridUnitType.Star);
                    _viewModel.MessagesColumnWidth = new GridLength(2.5, GridUnitType.Star);
                    _viewModel.SelectedUserIndex = 0;
                }

                _viewModel.BaseConfiguration.ChatService.SwitchChat(currentChatType, _viewModel.CurrentUser);
                _viewModel.CanCloseChat = false;

                Thread.Sleep(100);
            }
        }
    }
}
