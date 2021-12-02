using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.MVVM.ViewModels.ChatMainViewModels;
using SharedItems.Enums;
using SharedItems.Models;

namespace ChatClient.Commands
{
    public class UpdatePrivateMessagesCommand : CommandBase
    {
        private readonly ChatViewModel _viewModel;

        public UpdatePrivateMessagesCommand(ChatViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            if (_viewModel.SelectedUserIndex != -1 && _viewModel.CurrentChatType == ChatType.Private && parameter is UserModel SelectedUser )
            {
                _viewModel.BaseConfiguration.ChatService.UpdatePrivateMessages(SelectedUser, _viewModel.CurrentUser);
            }
        }
    }
}
