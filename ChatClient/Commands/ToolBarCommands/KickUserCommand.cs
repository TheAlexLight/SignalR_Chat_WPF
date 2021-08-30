using ChatClient.Services;
using ChatClient.ViewModels;
using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Commands.ToolBarCommands
{
    public class KickUserCommand : CommandBase
    {
        private readonly ChatViewModel _viewModel;

        public override void Execute(object parameter)
        {
            UserProfileModel selectedModel = parameter as UserProfileModel;

            if (_viewModel.CurrentUser.Username == selectedModel.Username)
            {
                NavigationService<LoginViewModel> navigationService = new(_viewModel.NavigationStore,
                        () => new LoginViewModel(_viewModel.NavigationStore, _viewModel.ChatService));

                navigationService.Navigate();
                //_viewModel.ChatService.
            }

            throw new NotImplementedException();
        }
    }
}
