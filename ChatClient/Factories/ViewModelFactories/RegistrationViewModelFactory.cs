using ChatClient.Interfaces;
using ChatClient.Services.BaseConfiguration;
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
        private readonly ChatBaseConfiguration _baseConfiguration;

        public RegistrationViewModelFactory(ChatBaseConfiguration baseConfiguration)
        {
            _baseConfiguration = baseConfiguration;
        }

        public RegistrationViewModel CreateViewModel()
        {
            return new RegistrationViewModel(_baseConfiguration);
        }
    }
}
