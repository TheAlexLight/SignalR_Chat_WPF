using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatClient.MVVM.ViewModels.ChatFeaturesModels;
using ChatClient.Services.BaseConfiguration;
using ChatClient.Services.ConcreteConfiguration;
using SharedItems.ViewModels;

namespace ChatClient.Commands.NavigatonCommands
{
    public class NavigateCommand<TViewModel> : CommandBase
        where TViewModel : ViewModelBase
    {
        private readonly ChatBaseConfiguration baseConfiguration;
        private readonly object[] _viewModelArgs;

        public NavigateCommand(ChatBaseConfiguration baseConfiguration, params object[] viewModelArgs)
        {
            this.baseConfiguration = baseConfiguration;
            _viewModelArgs = viewModelArgs;
        }

        public override void Execute(object parameter)
        {
           baseConfiguration.CreateConcreteNavigationService<TViewModel>(_viewModelArgs);

            baseConfiguration.NavigationService.Navigate();
        }
    }
}
