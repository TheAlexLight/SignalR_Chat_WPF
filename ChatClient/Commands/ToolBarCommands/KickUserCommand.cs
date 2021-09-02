using ChatClient.Services;
using ChatClient.ViewModels;
using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.Commands.ToolBarCommands
{
    public class KickUserCommand : CommandBase
    {
        private readonly ChatViewModel _viewModel;

        public KickUserCommand(ChatViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override async void Execute(object parameter)
        {
            if (parameter is UserProfileModel)
            {
                UserProfileModel selectedModel = parameter as UserProfileModel;

                if (_viewModel.CurrentUser.Username != selectedModel.Username)
                {
                    await _viewModel.ChatService.KickUser(selectedModel.Username);
                }
            }
        }
    }
}
