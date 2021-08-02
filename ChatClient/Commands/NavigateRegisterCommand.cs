using ChatClient.Stores;
using ChatClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Commands
{
    public class NavigateRegisterCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public NavigateRegisterCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            _navigationStore.CurrentViewModel = new RegisterViewModel(_navigationStore);
        }
    }
}
