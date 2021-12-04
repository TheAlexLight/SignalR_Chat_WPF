using SharedItems.Models;
using SharedItems.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.MVVM.ViewModels.ChatFeaturesModels
{
    public class ChatGroupViewModel : ViewModelBase
    {
        private ChatGroupModel _currentChatGroupModel;
        private ObservableCollection<MessageViewModel> _messagesViewModel;

        public ChatGroupViewModel()
        {
            MessagesViewModel = new();
        }

        public ChatGroupModel CurrentChatGroupModel
        {
            get => _currentChatGroupModel;
            set
            {
                _currentChatGroupModel = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MessageViewModel> MessagesViewModel
        {
            get => _messagesViewModel; 
            set
            {
                _messagesViewModel = value;
                OnPropertyChanged();
            }
        }
    }
}
