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
            if (parameter is UserModel user && user.UserProfile.IsOnline == true)
            {
                UserModel selectedModel = parameter as UserModel;

                if (_viewModel.CurrentUser.UserProfile.Username != selectedModel.UserProfile.Username)
                {
                    await _viewModel.BaseConfiguration.ChatService.KickUser(selectedModel.UserProfile.Username);
                }
            }
        }
    }
}
