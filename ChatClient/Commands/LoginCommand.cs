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
    class LoginCommand : CommandBase
    {
        private readonly LoginViewModel _viewModel;
        private readonly NavigationService<ChatViewModel> _navigationService;

        public LoginCommand( LoginViewModel viewModel, NavigationService<ChatViewModel> navigationService)
        {
            _viewModel = viewModel;
            _navigationService = navigationService;
        }
        public override async void Execute(object parameter)
        {
            MessageBox.Show($"Logging in {_viewModel.Username}...");
            await _viewModel.ChatService.Login(_viewModel.Username);
            //_navigationService.Navigate();
        }

    }
}
