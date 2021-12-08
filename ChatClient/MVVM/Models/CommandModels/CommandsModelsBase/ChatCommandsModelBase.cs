using ChatClient.Commands;
using ChatClient.MVVM.ViewModels.ChatMainViewModels;
using ChatClient.Services.ConcreteConfiguration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatClient.MVVM.Models.CommandModels.CommandModelsBase
{
    public class ChatCommandsModelBase
    {
        public ChatCommandsModelBase(ChatViewModel viewModel, SignalRChatService chatService)
        {
            AdminCommandsModel = new AdminCommandsModel(viewModel);
            RemoveToolBarOverflowCommand = new RemoveToolBarOverfowCommand();
            UserCredentialsCommandModel = new UserCredentialsCommandModel(viewModel);
            MessageCommandModel = new MessageCommandModel(viewModel, chatService);
            WindowSwitchingCommandModel = new WindowSwitchingCommandModel(viewModel);
        }

        public ICommand RemoveToolBarOverflowCommand { get; }

        public MessageCommandModel MessageCommandModel { get;}

        public UserCredentialsCommandModel UserCredentialsCommandModel { get; }

        public WindowSwitchingCommandModel WindowSwitchingCommandModel { get; }

        public AdminCommandsModel AdminCommandsModel { get;}
        
    }
}
