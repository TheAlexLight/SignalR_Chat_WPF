using ChatClient.Commands.NavigationCommands;
using ChatClient.Enums;
using ChatClient.Interfaces;
using ChatClient.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatClient.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigator _navigator;
        private readonly IViewModelAbstractFactory _viewModelFactory;

        public MainViewModel(INavigator navigator, IViewModelAbstractFactory viewModelFactory)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;

            _navigator.CurrentViewModelChanged += OnCurrentViewModelChanged;

            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(_navigator, _viewModelFactory);
            UpdateCurrentViewModelCommand.Execute(ViewType.Login);
        }

        private readonly ICommand UpdateCurrentViewModelCommand;

        public ViewModelBase CurrentViewModel => _navigator.CurrentViewModel;

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
