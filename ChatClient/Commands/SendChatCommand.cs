using ChatClient.Services;
using ChatClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatClient.Commands
{
    public class SendChatCommand : ICommand
    {
        private readonly ChatViewModel _viewModel;
        private readonly SignalRChatService _chatService;

        public SendChatCommand(ChatViewModel viewModel, SignalRChatService chatService)
        {
            _viewModel = viewModel;
            _chatService = chatService;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            try
            {
                await _chatService.SendMessage(_viewModel.Message);

                _viewModel.ErrorMessage = string.Empty;
            }
            catch (Exception)
            {
                _viewModel.ErrorMessage = "Unable to send message.";
            }
        }
    }
}
