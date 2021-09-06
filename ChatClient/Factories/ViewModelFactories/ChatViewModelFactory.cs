using ChatClient.Interfaces;
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

        public ChatViewModelFactory(INavigator navigator, ISignalRChatService chatService)
        {
            _navigator = navigator;
            _chatService = chatService;
        }

        public ChatViewModel CreateViewModel()
        {
            return new ChatViewModel(_navigator, _chatService);
        }
    }
}
