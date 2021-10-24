using SharedItems.Enums;
using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChatClient.Extensions
{
    public class ScrollViewerExtension : ScrollViewer
    {
        private bool _messagesHandled;

        public static readonly DependencyProperty MessageCollectionProperty;
        
        static ScrollViewerExtension()
        {
            MessageCollectionProperty = DependencyProperty.Register(nameof(MessageCollection)
            , typeof(List<MessageModel>), typeof(ScrollViewerExtension), new PropertyMetadata(new List<MessageModel>(), MessagesChanged));
        }

        private static void MessagesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewerExtension scroll && !scroll._messagesHandled)
            {
                if (scroll.MessageCollection != null && scroll.MessageCollection.Count != 0)
                {
                    scroll.Dispatcher.BeginInvoke(new Action(scroll.RenderingDone), System.Windows.Threading.DispatcherPriority.ContextIdle, null);
                }
            }
        }

        private void RenderingDone()
        {
            double messagesHeightCount = MessageCollection.Where(m => m.CheckStatus == MessageStatus.Read).Select(m => m.MessageHeight).Sum();
            ScrollToVerticalOffset(messagesHeightCount - ViewportHeight);
            _messagesHandled = true;
        }

        public List<MessageModel> MessageCollection
        {
            get => (List<MessageModel>)GetValue(MessageCollectionProperty);
            set => SetValue(MessageCollectionProperty, value);
        }
    }
}
