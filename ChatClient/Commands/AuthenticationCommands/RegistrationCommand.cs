using ChatClient.Services;
using ChatClient.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using SharedItems.Models;
using SharedItems.Models.AuthenticationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Commands.AuthenticationCommands
{
    public class RegistrationCommand : CommandBase
    {
        private readonly RegistrationViewModel _viewModel;
        private readonly SignalRChatService _chatService;

        public RegistrationCommand(RegistrationViewModel viewModel, SignalRChatService chatService)
        {
            _viewModel = viewModel;
            _chatService = chatService;
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
                UserRegistrationModel userData = new()
                {
                    Username = _viewModel.Username,
                    Email = _viewModel.Email,
                    Password = _viewModel.Password,
                    PasswordConfirm = _viewModel.PasswordConfirm,
                };

                await _chatService.Registration(userData);
            }

            _viewModel.IsLoading = false;
            RaiseCanExecuteChanged();
        }
    }
}
