using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using ChatClient.MVVM.ViewModels.ChatMainViewModels;
using SharedItems.Models;

namespace ChatClient.Commands.CustomViewsCommands
{
    public class OpenSettingsCommand : CommandBase
    {
        private readonly ChatViewModel _viewModel;

        public OpenSettingsCommand(ChatViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            object[] values = (object[])parameter;

            if (values[0] is string temporarySettingsState)
            {
                if (bool.TryParse(temporarySettingsState, out bool userSettingsState))
                {
                    _viewModel.IsSettingsOpened = userSettingsState;

                    Window window = Application.Current.MainWindow;

                    window.MinHeight = userSettingsState ? 450 : 350;
                }
            }

            if (values.Length == 2 && values[1] is UserModel selectedUser)
            {
                _viewModel.SelectedUser = selectedUser;
            }
        }
    }
}
