using ChatClient.Services;
using ChatClient.Stores;
using ChatClient.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.Commands
{
    public class LoginCommand : CommandBase
    {
        private readonly LoginViewModel _viewModel;

        public LoginCommand( LoginViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return UserStatusService.IsLogin;
        }

        public override async void Execute(object parameter)
        {
            //await _viewModel.ChatService.Login(_viewModel.Username);
        }
    }
}
