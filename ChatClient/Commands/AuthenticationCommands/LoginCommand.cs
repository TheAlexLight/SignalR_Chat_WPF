using ChatClient.Services;
using ChatClient.Stores;
using ChatClient.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.Commands.AuthenticationCommands
{
    public class LoginCommand : CommandBase
    {
        private readonly LoginViewModel _viewModel;

        public LoginCommand( LoginViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override async void Execute(object parameter)
        {
            LoginUserData loginData = new()
            {
                Username = _viewModel.Username,
                Password = _viewModel.Password
            };

            await _viewModel.ChatService.Login(loginData);
        }
    }
}
