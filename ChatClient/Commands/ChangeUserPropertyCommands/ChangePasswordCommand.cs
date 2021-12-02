using ChatClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Commands.ChangeUserPropertyCommand
{
    public class ChangePasswordCommand : CommandBase
    {
        private readonly ChatViewModel _viewModel;

        public ChangePasswordCommand(ChatViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            if (parameter is string password)
            {
                _viewModel.BaseConfiguration.ChatService.SubmitPasswordChange(_viewModel.CurrentUser, _viewModel.Password, password);
            }
        }
    }
}
