using SharedItems.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedItems.Models
{
    public class MessageInformationModel : ViewModelBase
    {
        private string _textMessage;
        private byte[] _imageMessage;
        private byte[] _videoMessage;

        public int Id { get; set; }
        public int MessageModelId { get; set; }

        public string TextMessage
        {
            get => _textMessage;
            set
            {
                _textMessage = value;
                OnPropertyChanged();
            }
        }
        public byte[] ImageMessage
        {
            get => _imageMessage;
            set
            {
                _imageMessage = value;
                OnPropertyChanged();
            }
        }
        public byte[] VideoMessage
        {
            get => _videoMessage;
            set
            {
                _videoMessage = value;
                OnPropertyChanged();
            }
        }
    }
}
