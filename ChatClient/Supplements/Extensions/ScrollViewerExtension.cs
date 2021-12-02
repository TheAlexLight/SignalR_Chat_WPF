using SharedItems.Enums;
using SharedItems.Models;
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

namespace ChatClient.Supplements.Extensions
{
    public class ScrollViewerExtension : ScrollViewer
    {
        ////private bool _messagesHandled;
        private bool _autoScroll = true;
        ////private Dictionary<string, double> _verticalOffsets = new Dictionary<string, double>();
        ////private string _currentKey;
        //private bool _isCollectionChanged;
        //private double _verticalOffset;
        //private bool _scrollPositionUpdateRequired;

        //public static readonly DependencyProperty MessageCollectionProperty;
        //public static readonly DependencyProperty ResetScrollPositionProperty;

        public static readonly RoutedEvent ScrollDownEvent = EventManager.RegisterRoutedEvent(
       "ScrollDown", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollViewerExtension));

        public event RoutedEventHandler ScrollDown
        {
            add { AddHandler(ScrollDownEvent, value); }
            remove { RemoveHandler(ScrollDownEvent, value); }
        }

        public ScrollViewerExtension()
        {
            ScrollDown += ScrollViewerExtension_ScrollDown;
        }

        private void ScrollViewerExtension_ScrollDown(object sender, RoutedEventArgs e)
        {
            this.ScrollToEnd();
        }

        public virtual void RaiseScrollDownEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs(ScrollDownEvent);
            RaiseEvent(args);
        }

        static ScrollViewerExtension()
        {
            //MessageCollectionProperty = DependencyProperty.Register(nameof(MessageCollection)
            //        , typeof(List<MessageModel>), typeof(ScrollViewerExtension), new PropertyMetadata(new List<MessageModel>(), MessagesChanged));

            //ResetScrollPositionProperty = DependencyProperty.RegisterAttached("ResetScrollPosition"
            //        , typeof(string), typeof(ScrollViewerExtension), new PropertyMetadata(string.Empty, ResetScrollPositionChanged));
        }

        //public static string GetResetScrollPosition(DependencyObject target)
        //{
        //    return (string)target.GetValue(ResetScrollPositionProperty);
        //}

        //public static void SetResetScrollPosition(DependencyObject target, bool value)
        //{
        //    target.SetValue(ResetScrollPositionProperty, value);
        //}

        //private static void ResetScrollPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    //if (d is ScrollViewerExtension scroll && e.NewValue != null)
        //    //{
        //    //    scroll._currentKey = (string)e.NewValue;
        //    //    string lastKey = (string)e.OldValue;

        //    //    string chosenKey = lastKey ?? scroll._currentKey;

        //    //    if (scroll._verticalOffsets.ContainsKey(chosenKey))
        //    //    {
        //    //        scroll._verticalOffsets[chosenKey] = scroll.VerticalOffset;/*scroll.VerticalOffset + scroll.ViewportHeight*/;
        //    //    }
        //    //    else
        //    //    {
        //    //        scroll._verticalOffsets.Add(chosenKey, scroll.VerticalOffset);// + scroll.ViewportHeight);
        //    //    }
        //}

        //private static void MessagesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    //if (d is ScrollViewerExtension scroll && !scroll._messagesHandled)
        //    //{
        //    //    if (scroll.MessageCollection != null && scroll.MessageCollection.Count != 0)
        //    //    {
        //    //        scroll.Dispatcher.BeginInvoke(new Action(scroll.RenderingDone), System.Windows.Threading.DispatcherPriority.ContextIdle, null);
        //    //    }
        //    //}
        //}

        ////private void RenderingDone()
        ////{
        ////    double messagesHeightCount = MessageCollection.Where(m => m.CheckStatus == MessageStatus.Read).Select(m => m.MessageHeight).Sum();
        ////    ScrollToVerticalOffset(messagesHeightCount - ViewportHeight);
        ////    _messagesHandled = true;
        ////}
        ///

        protected override void OnScrollChanged(ScrollChangedEventArgs e)
        {
            //if (_isCollectionChanged && _verticalOffsets.ContainsKey(_currentKey))
            //{
            //        ScrollToVerticalOffset(_verticalOffsets[_currentKey]);
            //        _isCollectionChanged = false;
            //}
            //else if(_isCollectionChanged)
            //{
            //    ScrollToVerticalOffset(0);
            //    _isCollectionChanged = false;
            //}

            // User scroll event : set or unset auto-scroll mode
            if (e.ExtentHeightChange == 0)
            {   // Content unchanged : user scroll event
                if (VerticalOffset == ScrollableHeight)
                {   // Scroll bar is in bottom
                    // Set auto-scroll mode
                    _autoScroll = true;
                }
                else
                {   // Scroll bar isn't in bottom
                    // Unset auto-scroll mode
                    _autoScroll = false;
                }
            }

            // Content scroll event : auto-scroll eventually
            if (_autoScroll && e.ExtentHeightChange != 0)
            {   // Content changed and auto-scroll mode set
                // Autoscroll
                ScrollToVerticalOffset(ExtentHeight);
            }

            base.OnScrollChanged(e);
        }

        //public List<MessageModel> MessageCollection
        //{
        //    get => (List<MessageModel>)GetValue(MessageCollectionProperty);
        //    set => SetValue(MessageCollectionProperty, value);
        //}
    }
}
