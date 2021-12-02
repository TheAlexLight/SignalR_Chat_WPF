using ChatClient.Interfaces;
using ChatClient.Services;
using ChatClient.Services.BaseConfiguration;
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
        private readonly ChatBaseConfiguration _baseConfiguration;

        public ChatViewModelFactory(ChatBaseConfiguration baseConfiguration)
        {
            _baseConfiguration = baseConfiguration;
        }

        public ChatViewModel CreateViewModel()
        {
            return new ChatViewModel(_baseConfiguration);
        }
    }
}
