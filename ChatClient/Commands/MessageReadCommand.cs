using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ChatClient.MVVM.ViewModels.ChatFeaturesModels;
using ChatClient.MVVM.ViewModels.ChatMainViewModels;
using SharedItems.Enums;

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
            if (values[0] is MessageViewModel message 
                    && _viewModel.CurrentUser != null 
                    && message.MessageModel.Sender != _viewModel.CurrentUser.UserProfile.Username)
            {
                if (IsUserVisible((FrameworkElement)values[1], (FrameworkElement)values[2])
                    && message.MessageModel.CheckStatus == MessageStatus.Received)
                {
                    message.MessageModel.CheckStatus = MessageStatus.Read;
                    _viewModel.BaseConfiguration.ChatService.UpdateMessage(message.MessageModel, _viewModel.CurrentChatGroup.CurrentChatGroupModel);
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

            return rect.Contains(bounds.BottomRight) && rect.Contains(bounds.BottomLeft);
        }
    }
}
