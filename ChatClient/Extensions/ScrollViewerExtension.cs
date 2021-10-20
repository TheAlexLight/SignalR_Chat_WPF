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
            MessageHeightProperty = DependencyProperty.Register("MessageHeight"
           , typeof(double), typeof(ScrollViewerExtension), new PropertyMetadata(0.0, MessageHeightChanged));
        }

        private static void MessageHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private static void MessagesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewerExtension scroll && !scroll._messagesHandled)
            {
                if (scroll.MessageCollection != null && scroll.MessageCollection.Count != 0)
                {
                    int visibleCount = (int)(scroll.ActualHeight / scroll.MessageHeight);
                    int scipMessagesCount = scroll.MessageCollection.Where(m => m.CheckStatus == MessageStatus.Read).Count();
                    scroll.ScrollToVerticalOffset(scipMessagesCount);
                    scroll._messagesHandled = true;
                }
            }
        }

        public List<MessageModel> MessageCollection
        {
            get => (List<MessageModel>)GetValue(MessageCollectionProperty);
            set => SetValue(MessageCollectionProperty, value);
        }
        public double MessageHeight
        {
            get => (double)GetValue(MessageHeightProperty);
            set => SetValue(MessageHeightProperty, value);
        }

        protected override void OnScrollChanged(ScrollChangedEventArgs e)
        {
            if (!(_scrollOffset != 0 && ((e.VerticalChange <= 0 && e.ExtentHeightChange != 0)
                || (e.VerticalChange == 0 && e.ExtentHeightChange == 0))))
            {
                _scrollOffset = e.VerticalOffset;
            }
            else
            {
                _scrollViewerExtension.ScrollToVerticalOffset(_scrollOffset);
            }

            base.OnScrollChanged(e);
        }

        public static bool GetAlwaysScrollToEnd(ScrollViewer scroll) => (bool)scroll.GetValue(AlwaysScrollToEndProperty);
        public static void SetAlwaysScrollToEnd(ScrollViewer scroll, bool alwaysScrollToEnd) => scroll.SetValue(AlwaysScrollToEndProperty, alwaysScrollToEnd);

        //public static double GetMessageHeight(UIElement element) => (double)element.GetValue(MessageHeightProperty);
        //public static void SetMessageHeight(UIElement element, double messageHeight) => element.SetValue(MessageHeightProperty, messageHeight);


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
