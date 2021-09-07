using ChatClient.Interfaces;
using ChatClient.Services;
using ChatClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Factories.ViewModelFactories
{
    public class ChatViewModelFactory : IViewModelConcreteFactory<ChatViewModel>
    {
        private readonly INavigator _navigator;
        private readonly ISignalRChatService _chatService;
        private readonly IWindowConfigurationService _windowConfigurationService;

        public ChatViewModelFactory(INavigator navigator, ISignalRChatService chatService
                , IWindowConfigurationService windowConfigurationService)
        {
            _navigator = navigator;
            _chatService = chatService;
            _windowConfigurationService = windowConfigurationService;
        }

        public ChatViewModel CreateViewModel()
        {
            return new ChatViewModel(_navigator, _chatService, _windowConfigurationService);
        }
    }
}
