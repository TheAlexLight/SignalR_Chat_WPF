using ChatClient.Services;
using ChatClient.ViewModels;
using SharedItems.Models;
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

        public override async void Execute(object parameter)
        {
            RegistrationUserData userData = new()
            {
                Username = _viewModel.Username,
                Email = _viewModel.Email,
                Password = _viewModel.Password,
                PasswordConfirm = _viewModel.PasswordConfirm,
                JoinDate = DateTime.Now
            };
 
            await _chatService.Registration(userData);
        }
    }
}
