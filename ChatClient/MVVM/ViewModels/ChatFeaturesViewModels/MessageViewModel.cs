using SharedItems.Models;
using SharedItems.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ChatClient.ViewModels
{
    public class MessageViewModel : ViewModelBase
    {
        public MessageViewModel(MessageModel messageModel)
        {
            MessageModel = messageModel;
            HyperlinkPanelVisibility = MessageModel.HyperlinkDescriptionModel.HyperlinkTitle == null ? Visibility.Collapsed : Visibility.Visible;
        }

        private Visibility _hyperlinkPanelVisibility;

        public MessageModel MessageModel { get; set; }

        public Visibility HyperlinkPanelVisibility
        {
            get => _hyperlinkPanelVisibility;
            set
            {
                _hyperlinkPanelVisibility = value;
                OnPropertyChanged();
            }
        }
    }
}
