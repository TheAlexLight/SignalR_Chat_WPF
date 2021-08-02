using ChatClient.Commands;
using ChatClient.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatClient.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        public ChatViewModel()
        {

        }
        public ChatViewModel(SignalRChatService chatService)
        {
            _chatService = chatService;
            //SendChatMessageCommand = new SendChatCommand(this, chatService);

            Messages = new();

            chatService.MessageReceived += ChatService_MessageReceived;
        }

        private string _message;
        private string _errorMessage;
        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        public ObservableCollection<string> Messages { get; }

        private ICommand sendChatMessageCommand;

        private SignalRChatService _chatService;


        public ICommand SendChatMessageCommand
        {
            get
            {
                return sendChatMessageCommand ??= new RelayCommand(async parameter =>
                  {
                      try
                      {
                          await _chatService.SendMessage(Message);

                          ErrorMessage = string.Empty;
                      }
                      catch (Exception)
                      {
                          ErrorMessage = "Unable to send message.";
                      }
                  });
            }
            set
            {

            }
        }

        public static ChatViewModel CreateConnectedViewModel(SignalRChatService chatService)
        {
            ChatViewModel viewModel = new(chatService);

            chatService.Connect().ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    viewModel.ErrorMessage = "Unable to connect to chat hub";
                }
            });

            return viewModel;
        }

        private void ChatService_MessageReceived(string message)
        {
            Messages.Add(message);
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

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

        
        //public ICommand SendChatMessageCommand { get; }

    }
}
