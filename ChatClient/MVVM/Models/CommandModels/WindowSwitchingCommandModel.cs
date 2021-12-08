using ChatClient.Commands;
using ChatClient.Commands.CustomViewsCommands;
using ChatClient.MVVM.ViewModels.ChatMainViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatClient.MVVM.Models.CommandModels
{
    public class WindowSwitchingCommandModel
    {
        public WindowSwitchingCommandModel(ChatViewModel viewModel)
        {
            SwitchChatCommand = new SwitchChatCommand(viewModel);
            OpenSettingsCommand = new OpenSettingsCommand(viewModel);
            OpenUserInfoWIndowCommand = new OpenUserInfoWIndowCommand(viewModel);
        }

        public ICommand SwitchChatCommand { get;}
        public ICommand OpenUserInfoWIndowCommand { get;}
        public ICommand OpenSettingsCommand { get;}
    }
}
