using ChatClient.Commands.ContextMenuCommands;
using ChatClient.Commands.ToolBarCommands;
using ChatClient.MVVM.ViewModels.ChatMainViewModels;
using SharedItems.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatClient.MVVM.Models.CommandModels
{
    public class AdminCommandsModel
    {
        public AdminCommandsModel(ChatViewModel viewModel)
        {
            BanUserCommand = new BanUserCommand(viewModel);
            MuteUserCommand = new MuteUserCommand(viewModel);
            KickUserCommand = new KickUserCommand(viewModel);
        }

        public ICommand BanUserCommand { get;}
        public ICommand MuteUserCommand { get;}
        public ICommand KickUserCommand { get; }
    }
}
