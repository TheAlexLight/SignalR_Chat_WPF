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
    public class ViewModelFactory : IViewModelAbstractFactory
    {
        private readonly IViewModelConcreteFactory<LoginViewModel> _loginViewModelFactory;
        private readonly IViewModelConcreteFactory<RegistrationViewModel> _registrationViewModelFactory;
        private readonly IViewModelConcreteFactory<ChatViewModel> _chatViewModelFactory;

        public ViewModelFactory(IViewModelConcreteFactory<LoginViewModel> loginViewModelFactory
                , IViewModelConcreteFactory<RegistrationViewModel> registrationViewModelFactory
                , IViewModelConcreteFactory<ChatViewModel> chatViewModelFactory)
        {
            _loginViewModelFactory = loginViewModelFactory;
            _registrationViewModelFactory = registrationViewModelFactory;
            _chatViewModelFactory = chatViewModelFactory;
        }

        public ViewModelBase CreateViewModel(ViewType viewType)
        {
            return viewType switch
            {
                ViewType.Login => _loginViewModelFactory.CreateViewModel(),
                ViewType.Registration => _registrationViewModelFactory.CreateViewModel(),
                ViewType.Chat => _chatViewModelFactory.CreateViewModel(),
                _ => throw new ArgumentException("The ViewType does not have a ViewModel", nameof(viewType)),
            };
        }
    }
}
