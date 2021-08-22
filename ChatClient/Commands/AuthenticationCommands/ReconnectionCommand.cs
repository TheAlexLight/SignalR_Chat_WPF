using ChatClient.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Commands.AuthenticationCommands
{
    public class ReconnectionCommand : CommandBase
    {
        private readonly ChatViewModel _viewModel;

        public ReconnectionCommand(ChatViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return true;//!_viewModel.IsLoading;
        }

        public override async void Execute(object parameter)
        {
            _viewModel.IsLoading = true;
            RaiseCanExecuteChanged();

            if (await _viewModel.ConnectToServer(_viewModel) != HubConnectionState.Disconnected)
            {
                await _viewModel.ChatService.Reconnect(_viewModel.CurrentUser.Username);
            }

            _viewModel.IsLoading = false;
            RaiseCanExecuteChanged();
        }
    }
}
