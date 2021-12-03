using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.MVVM.ViewModels.ChatMainViewModels;

namespace ChatClient.Commands.ChangeUserPropertyCommand
{
    public class ChangeUsernameCommand : CommandBase
    {
        private readonly ChatViewModel _viewModel;

        public ChangeUsernameCommand(ChatViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            if (parameter is string password)
            {
                _viewModel.BaseConfiguration.ChatService.CredentialModel.ChangeUsername(_viewModel.CurrentUser, _viewModel.UsernameSettingsField, password);
            }
        }
    }
}
