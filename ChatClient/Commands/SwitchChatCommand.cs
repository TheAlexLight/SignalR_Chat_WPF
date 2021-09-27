using ChatClient.Enums;
using ChatClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Commands
{
    public class SwitchChatCommand : CommandBase
    {
        private readonly ChatViewModel viewModel;

        public SwitchChatCommand(ChatViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            if (parameter is ChatType currentChatCtype)
            {
                viewModel.CurrentChatType = currentChatCtype;
            }
        }
    }
}
