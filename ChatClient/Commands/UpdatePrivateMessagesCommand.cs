using ChatClient.ViewModels;
using SharedItems.Enums;
using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if ( _viewModel.CurrentChatType == ChatType.Private && parameter is UserModel SelectedUser )
            {
                _viewModel.ChatService.UpdatePrivateMessages(SelectedUser, _viewModel.CurrentUser);
            }
        }
    }
}
