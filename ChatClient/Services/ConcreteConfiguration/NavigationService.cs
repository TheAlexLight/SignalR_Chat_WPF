using ChatClient.Interfaces;
using ChatClient.Stores;
using ChatClient.ViewModels;
using SharedItems.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Services
{
    public class NavigationService<TViewModel>
        where TViewModel : ViewModelBase
    {
        private readonly INavigator _navigator;
        private readonly Func<TViewModel> _createViewModel;

        public NavigationService(INavigator navigator, Func<TViewModel> createViewModel )
        {
            _navigator = navigator;
            _createViewModel = createViewModel;
        }
        
        public void Navigate() 
        {
            _navigator.CurrentViewModel = _createViewModel();
        }
    }
}
