using ChatClient.Enums;
using ChatClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.ViewModels
{
   public class ChatViewModelBase : ViewModelBase
    {
        protected readonly SignalRChatService _chatService;

        public ChatViewModelBase(SignalRChatService chatService)
        {
            _chatService = chatService;
            _chatService.Connection.Reconnecting += Connection_Reconnecting;
        }

        protected virtual Task Connection_Reconnecting(Exception arg)
        {
            throw new NotImplementedException(); //ToDo: implement
        }

        private string _errorMessage;
        private ConnectionStatus _connectionStatusValue;

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }

        public ConnectionStatus ConnectionStatusValue
        {
            get => _connectionStatusValue;
            set
            {
                _connectionStatusValue = value;
                OnPropertyChanged();
            }
        }
    }
}
