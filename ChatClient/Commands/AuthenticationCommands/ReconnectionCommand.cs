using ChatClient.Commands.NavigatonCommands;
using ChatClient.Services;
using ChatClient.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using SharedItems.Models;
using SharedItems.Models.StatusModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatClient.Commands.AuthenticationCommands
{
    public class ReconnectionCommand : CommandBase
    {
        private readonly ChatViewModelBase _viewModel;
        private readonly UserProfileModel _currentUser;
        private readonly ICommand _navigationCommand;

        public ReconnectionCommand(ChatViewModelBase viewModel, UserProfileModel currentUser)
        {
            _viewModel = viewModel;
            _currentUser = currentUser;

            _navigationCommand = new NavigateCommand<ChatViewModel>(new NavigationService<ChatViewModel>(_viewModel.Navigator,
                    () => new ChatViewModel(_viewModel.Navigator, _viewModel.ChatService, _viewModel.WindowConfigurationService)));
        }

        public override bool CanExecute(object parameter)
        {
            return !_viewModel.IsLoading;
        }

        public override async void Execute(object parameter)
        {
            _viewModel.IsLoading = true;
            RaiseCanExecuteChanged();

            if (await _viewModel.ConnectToServer(_viewModel) != HubConnectionState.Disconnected)
            {
                await _viewModel.ChatService.Reconnect(_currentUser.Username);

                if (parameter is BanStatusModel banStatus)
                {
                    await _viewModel.ChatService.SendBan(_currentUser.Username, banStatus);
                }
            }

            _navigationCommand.Execute(null);

            _viewModel.IsLoading = false;
            RaiseCanExecuteChanged();
        }
    }
}
