using AutoMapper;
using ChatClient.Interfaces;
using ChatClient.Interfaces.BaseConfiguration;
using ChatClient.MVVM.ViewModels.ChatMainViewModels;
using ChatClient.Services.ConcreteConfiguration;
using SharedItems.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Services.BaseConfiguration
{
    public class ChatBaseConfiguration : IChatBaseConfiguration
    {
        public ChatBaseConfiguration(INavigator navigator, SignalRChatService chatService, IWindowConfigurationService windowConfigurationService
                /*,IMapper mapper*/)
        {
            Navigator = navigator;
            ChatService = chatService;
            WindowConfigurationService = windowConfigurationService;
            //Mapper = mapper;
        }

        public INavigator Navigator { get; }
        public SignalRChatService ChatService { get; }
        public IWindowConfigurationService WindowConfigurationService { get; }
        public INavigationService NavigationService { get; private set; }
        //public IMapper Mapper { get; set; }

        public void CreateConcreteNavigationService<TViewModel>(params object[] args) where TViewModel : ViewModelBase
        {
            try
            {
                var viewModel = (TViewModel)Activator.CreateInstance(typeof(TViewModel), args);

                NavigationService = new NavigationService<TViewModel>(Navigator, () => viewModel);
            }
            catch (MissingMethodException ex)
            { 
                throw new MissingMethodException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
