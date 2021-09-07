using ChatClient.Enums;
using ChatClient.Interfaces;
using ChatClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Factories.ViewModelFactories
{
    public class LoginViewModelFactory : IViewModelConcreteFactory<LoginViewModel>
    {
        private readonly INavigator _navigator;
        private readonly ISignalRChatService _chatService;
        private readonly IWindowConfigurationService _windowConfigurationService;

        public LoginViewModelFactory(INavigator navigator, ISignalRChatService chatService
                ,IWindowConfigurationService windowConfigurationService)
        {
            _navigator = navigator;
            _chatService = chatService;
            _windowConfigurationService = windowConfigurationService;
        }

        public LoginViewModel CreateViewModel()
        {
            return new LoginViewModel(_navigator, _chatService, _windowConfigurationService);
        }
    }
}
