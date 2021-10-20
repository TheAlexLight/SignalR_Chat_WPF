using SharedItems.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedItems.Models
{
    public class MessageModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private MessageStatus _checkStatus;
        private double _messageHeight;

        public int Id { get; set; }
        public int UserModelId { get; set; }
        public int ChatGroupModelId { get; set; }

        public DateTime Time { get; set; }
        public bool IsFirstMessage { get; set; }
        public MessageStatus CheckStatus 
        {
            get => _checkStatus;
            set
            {
                _checkStatus = value;
                OnPropertyChanged();
            }
        }

        public double MessageHeight
        {
            get => _messageHeight;
            set
            {
                if(value != 0)
                {
                    _messageHeight = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Count { get; set; }
        public string Message { get; set; }
        [JsonIgnore]
        public virtual UserModel UserModel { get; set; }

        [JsonIgnore]
        public virtual ChatGroupModel ChatGroupModel { get; set; }
    }
}
