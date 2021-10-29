using ChatClient.ViewModels;
using SharedItems.Enums;
using SharedItems.Models;
using SharedItems.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChatClient.Commands
{
    public class MessageReadCommand : CommandBase
    {
        private readonly ChatViewModel _viewModel;

        public MessageReadCommand(ChatViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            object[] values = parameter as object[];
            if (values[0] is MessageModel message && message.Sender != _viewModel.CurrentUser.UserProfile.Username)
            {
                if (IsUserVisible((FrameworkElement)values[1], (FrameworkElement)values[2])
                    && message.CheckStatus == MessageStatus.Received)
                {
                    message.CheckStatus = MessageStatus.Read;
                    _viewModel.ChatService.UpdateMessage(message, _viewModel.CurrentChatGroup);
                }
            }
        }

        private bool IsUserVisible(FrameworkElement element, FrameworkElement container)
        {
            if (!element.IsVisible)
            {
                return false;
            }

            Rect bounds = element.TransformToAncestor(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
            Rect rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);

            return rect.Contains(bounds);
        }
    }
}
