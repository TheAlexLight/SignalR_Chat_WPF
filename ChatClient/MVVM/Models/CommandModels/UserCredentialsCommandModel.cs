using ChatClient.Commands;
using ChatClient.Commands.ChangeUserPropertyCommand;
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
    public class UserCredentialsCommandModel
    {
        public UserCredentialsCommandModel(ChatViewModel viewModel)
        {
            ChangeUsernameCommand = new ChangeUsernameCommand(viewModel);
            ChangeEmailCommand = new ChangeEmailCommand(viewModel);
            ChangePasswordCommand = new ChangePasswordCommand(viewModel);

            ChangePhotoCommand = new ChangePhotoCommand(viewModel);

            ChangeUserSettingsCommand = new ChangeUserSettingsCommand(viewModel);
        }

        public ICommand ChangeUsernameCommand { get;}
        public ICommand ChangeEmailCommand { get;}
        public ICommand ChangePasswordCommand { get;}

        public ICommand ChangePhotoCommand { get;}

        public ICommand ChangeUserSettingsCommand { get;}
    }
}
