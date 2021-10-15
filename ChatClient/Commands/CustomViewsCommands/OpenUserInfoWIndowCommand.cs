using ChatClient.ViewModels;
using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.Commands.CustomViewsCommands
{
    public class OpenUserInfoWIndowCommand : CommandBase
    {
        private readonly ChatViewModel _viewModel;

        public OpenUserInfoWIndowCommand(ChatViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            object[] values = (object[])parameter;

            if (values[0] is string temporaryUserInfoState)
            {
                if (bool.TryParse(temporaryUserInfoState, out bool userInfoOpenedState))
                {
                    _viewModel.IsUserInfoOpened = userInfoOpenedState;
                }
            }

            if (values.Length == 2 && values[1] is UserModel selectedUser)
            {
                _viewModel.SelectedUser = selectedUser;
            }
        }
    }
}
