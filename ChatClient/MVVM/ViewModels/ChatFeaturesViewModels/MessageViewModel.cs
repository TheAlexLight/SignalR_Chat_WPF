using ChatClient.Supplements.Extensions;
using SharedItems.Models;
using SharedItems.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ChatClient.MVVM.ViewModels.ChatFeaturesModels
{
    public class MessageViewModel : ViewModelBase
    {
        public MessageViewModel(MessageModel messageModel)
        {
            MessageModel = messageModel;
            HyperlinkPanelVisibility = MessageModel.HyperlinkDescriptionModel.HyperlinkTitle == null ? Visibility.Collapsed : Visibility.Visible;
        }

        private Visibility _hyperlinkPanelVisibility;

        private BitmapImage _image;

        private MessageModel _messageModel;

        public MessageModel MessageModel
        {
            get => _messageModel;
            set
            {
                _messageModel = value;
                OnPropertyChanged();
            }
        }

        public BitmapImage Image
        {
            get
            {
                _image = new();

                Stream _imageStream = new MemoryStream(MessageModel.UserModel.UserProfile.Image);

                _image.BeginInit();

                _image.StreamSource = _imageStream;
                _image.DecodePixelWidth = 50;

                _image.EndInit();

                return _image;
            }
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }

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
