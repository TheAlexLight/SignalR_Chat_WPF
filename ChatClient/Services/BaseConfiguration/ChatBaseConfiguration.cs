using ChatClient.Interfaces;
using ChatClient.Interfaces.BaseConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Services.BaseConfiguration
{
    public class ChatBaseConfiguration
    {
        public ChatBaseConfiguration(INavigator navigator, ISignalRChatService chatService, IWindowConfigurationService windowConfigurationService)
        {
            Navigator = navigator;
            ChatService = chatService;
            WindowConfigurationService = windowConfigurationService;
        }

        public INavigator Navigator { get; }
        public ISignalRChatService ChatService { get; }
        public IWindowConfigurationService WindowConfigurationService { get; }
        //public INavigationService NavigationService { get; }

    }
}
