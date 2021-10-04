using ChatClient.Enums;
using ChatClient.ViewModels;
using SharedItems.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (parameter is ChatType currentChatCtype)
            {
                _viewModel.CurrentChatType = currentChatCtype;
                _viewModel.ChatService.SwitchChat(currentChatCtype);
            }
        }
    }
}
