using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using ChatClient.MVVM.ViewModels.ChatMainViewModels;
using SharedItems.Models;

namespace ChatClient.Commands
{
    class MessageLoadedCommand : CommandBase
    {
        private readonly ChatViewModel _viewModel;

        public MessageLoadedCommand(ChatViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            object[] values = parameter as object[];

            if (values[0] is MessageModel message)
            {
                Thickness margin = new Thickness(int.Parse((string)values[2]));
                FrameworkElement control = (FrameworkElement)values[1];
                message.MessageHeight = control.ActualHeight + margin.Top + margin.Bottom;

                //_viewModel.ChatService.UpdateMessage(message);
            }
        }
    }
}
