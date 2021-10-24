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
        //// Create a custom routed event by first registering a RoutedEventID
        //// This event uses the bubbling routing strategy
        //public static readonly RoutedEvent CustomScrollChangedEvent = EventManager.RegisterRoutedEvent(
        //    nameof(CustomScrollChanged), RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(ScrollViewerExtension));

        //// Provide CLR accessors for the event
        //public event RoutedEventHandler CustomScrollChanged
        //{
        //    add { AddHandler(CustomScrollChangedEvent, value); }
        //    remove { RemoveHandler(CustomScrollChangedEvent, value); }
        //}

        //// This method raises the Tap event
        //void RaiseCustomScrollChangedEvent()
        //{
        //    RoutedEventArgs newEventArgs = new RoutedEventArgs(CustomScrollChangedEvent);
        //    RaiseEvent(newEventArgs);
        //}

        private bool _messagesHandled;
        private double _scrollOffset;
        private ScrollViewerExtension _scrollViewerExtension;

        public static readonly DependencyProperty AlwaysScrollToEndProperty;
        public static readonly DependencyProperty MessageCollectionProperty;
        public static readonly DependencyProperty MessageHeightProperty;
        
        static ScrollViewerExtension()
        {
            AlwaysScrollToEndProperty = DependencyProperty.RegisterAttached("AlwaysScrollToEnd"
              , typeof(bool), typeof(ScrollViewerExtension), new PropertyMetadata(false, AlwaysScrollToEndChanged));

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

        public static bool GetAlwaysScrollToEnd(ScrollViewer scroll) => (bool)scroll.GetValue(AlwaysScrollToEndProperty);
        public static void SetAlwaysScrollToEnd(ScrollViewer scroll, bool alwaysScrollToEnd) => scroll.SetValue(AlwaysScrollToEndProperty, alwaysScrollToEnd);

        private static void AlwaysScrollToEndChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ScrollViewerExtension scroll)
            {
                bool alwaysScrollToEnd = (e.NewValue != null) && (bool)e.NewValue;

                if (alwaysScrollToEnd)
                {
                    scroll._scrollViewerExtension = scroll;
                }
            }
            else
            {
                throw new InvalidOperationException("The attached AlwaysScrollToEnd property can only be applied to ScrollViewer instances.");
            }
        }
    }
}
