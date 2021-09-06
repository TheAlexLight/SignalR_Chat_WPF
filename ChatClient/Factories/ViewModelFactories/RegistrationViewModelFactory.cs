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

        public RegistrationViewModelFactory(INavigator navigator, ISignalRChatService chatService)
        {
            _navigator = navigator;
            _chatService = chatService;
        }

        public RegistrationViewModel CreateViewModel()
        {
            return new RegistrationViewModel(_navigator, _chatService);
        }
    }
}
