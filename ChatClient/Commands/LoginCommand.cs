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
        private readonly NavigationService<ChatViewModel> _navigationService;

        public LoginCommand( LoginViewModel viewModel, NavigationService<ChatViewModel> navigationService)
        {
            _viewModel = viewModel;
            _navigationService = navigationService;
        }

        public override event EventHandler CanExecuteChanged;

        public override bool CanExecute(object parameter)
        {
            return LoginConnectionService.IsLogin;
        }

        public override async void Execute(object parameter)
        {
            //MessageBox.Show($"Logging in {_viewModel.Username}...");

            await _viewModel.ChatService.Login(_viewModel.Username);
            //_navigationService.Navigate();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
