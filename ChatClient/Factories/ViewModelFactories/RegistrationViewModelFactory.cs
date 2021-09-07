using ChatClient.Interfaces;
using ChatClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Factories.ViewModelFactories
{
    public class RegistrationViewModelFactory : IViewModelConcreteFactory<RegistrationViewModel>
    {
        private readonly INavigator _navigator;
        private readonly ISignalRChatService _chatService;
        private readonly IWindowConfigurationService _windowConfigurationService;

        public RegistrationViewModelFactory(INavigator navigator, ISignalRChatService chatService
                , IWindowConfigurationService windowConfigurationService)
        {
            _navigator = navigator;
            _chatService = chatService;
            _windowConfigurationService = windowConfigurationService;
        }

        public RegistrationViewModel CreateViewModel()
        {
            return new RegistrationViewModel(_navigator, _chatService, _windowConfigurationService);
        }
    }
}
