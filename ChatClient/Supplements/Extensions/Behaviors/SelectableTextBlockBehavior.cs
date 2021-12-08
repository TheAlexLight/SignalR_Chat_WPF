using ChatClient.MVVM.ViewModels.ChatMainViewModels;
using ChatClient.Supplements.Helpers;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace ChatClient.Supplements.Extensions.Behaviors
{
    public class SelectableTextBlockBehavior : Behavior<TextBlock>
    {
        private TextPointer _startSelectPosition;
        private TextPointer _endSelectPosition;

        private string _selectedText = string.Empty;

        public static readonly DependencyProperty SelectedTextColorProperty;

        public static readonly DependencyProperty CopyTextProperty;
        public static readonly DependencyProperty DeleteMessageProperty;

        static SelectableTextBlockBehavior()
        {
            SelectedTextColorProperty = DependencyProperty.RegisterAttached("SelectedTextColor"
                    , typeof(SolidColorBrush), typeof(SelectableTextBlockBehavior), new PropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("#A2A6DF")));

            CopyTextProperty = DependencyProperty.RegisterAttached("CopyText", typeof(MenuItem), typeof(SelectableTextBlockBehavior), new PropertyMetadata(null, CopyTextChanged));
            
            DeleteMessageProperty = DependencyProperty.RegisterAttached("DeleteMessage", typeof(MenuItem), typeof(SelectableTextBlockBehavior), new PropertyMetadata(null, DeleteMessageChanged));
        }

        public MenuItem CopyText
        {
            get => (MenuItem)GetValue(CopyTextProperty);
            set => SetValue(CopyTextProperty, value);
        }

        public MenuItem DeleteMessage
        {
            get => (MenuItem)GetValue(DeleteMessageProperty);
            set => SetValue(DeleteMessageProperty, value);
        }

        private static void DeleteMessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SelectableTextBlockBehavior selectableTextBlock)
            {
                if (e.OldValue != null)
                {
                    ((MenuItem)e.OldValue).Click -= selectableTextBlock.DeleteMessage_Click;
                }

                ((MenuItem)e.NewValue).Click += selectableTextBlock.DeleteMessage_Click;
            }
        }

        private static void CopyTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SelectableTextBlockBehavior selectableTextBlock)
            {
                if (e.OldValue != null)
                {
                    ((MenuItem)e.OldValue).Click -= selectableTextBlock.CopyText_Click;
                }
                
                ((MenuItem)e.NewValue).Click += selectableTextBlock.CopyText_Click;
            }
        }

        public static SolidColorBrush GetSelectedTextColor(DependencyObject target)
        {
            return (SolidColorBrush)target.GetValue(SelectedTextColorProperty);
        }

        public static void SetSelectedTextColor(DependencyObject target, SolidColorBrush value)
        {
            target.SetValue(SelectedTextColorProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.KeyUp += AssociatedObject_KeyUp;
            this.AssociatedObject.LostKeyboardFocus += AssociatedObject_LostKeyboardFocus;

            this.AssociatedObject.MouseDown += AssociatedObject_MouseDown;
            this.AssociatedObject.MouseUp += AssociatedObject_MouseUp;
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
        }

        private void CopyText_Click(object sender, RoutedEventArgs e)
        {
           string text = HyperlinksDetetectionHelper.GetText(this.AssociatedObject) ?? this.AssociatedObject.Text;

            Clipboard.SetText(text);
        }

        private void DeleteMessage_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel mainWindowViewModel = (MainViewModel)Application.Current.MainWindow.DataContext;

            ChatViewModel chatViewModel = (ChatViewModel)mainWindowViewModel.CurrentViewModel;

            chatViewModel.CommandsModel.MessageCommandModel.DeleteMessageCommand.Execute(this.AssociatedObject.DataContext);
        }

        private void AssociatedObject_LostKeyboardFocus(object sender, RoutedEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;

            Point emptyPoint = new Point(0, 0);

            _startSelectPosition = textBlock.GetPositionFromPoint(emptyPoint, true);
            _endSelectPosition = textBlock.GetPositionFromPoint(emptyPoint, true);

            TextRange fullText = new TextRange(textBlock.ContentStart, textBlock.ContentEnd);
            fullText.ApplyPropertyValue(TextElement.BackgroundProperty, textBlock.Background);
            e.Handled = true;

            _startSelectPosition = null;
            _endSelectPosition = null;
        }

        private void AssociatedObject_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.C)
            {
                Clipboard.SetText(_selectedText);
            }
        }

        private void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                TextBlock textBlock = sender as TextBlock;

                if (_startSelectPosition != null)
                {
                    Point mouseUpPoint = e.GetPosition(textBlock);

                    _endSelectPosition = textBlock.GetPositionFromPoint(mouseUpPoint, true);

                    TextRange fullText = new TextRange(textBlock.ContentStart,textBlock.ContentEnd);
                    fullText.ApplyPropertyValue(TextElement.BackgroundProperty, textBlock.Background);

                    TextRange selectedText = new TextRange(_startSelectPosition, _endSelectPosition);
                    selectedText.ApplyPropertyValue(TextElement.BackgroundProperty, GetSelectedTextColor(textBlock)/*(SolidColorBrush)new BrushConverter().ConvertFrom("#7CCCDC")*/);

                    _selectedText = selectedText.Text;
                }
            }
        }

        private void AssociatedObject_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;

            textBlock.Focus();
            e.Handled = true;

            Point mouseDownPoint = e.GetPosition(textBlock);
            _startSelectPosition = textBlock.GetPositionFromPoint(mouseDownPoint, true);
        }

        private void AssociatedObject_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;

            if (_startSelectPosition != null)
            {
                Point mouseUpPoint = e.GetPosition(textBlock);
                _endSelectPosition = textBlock.GetPositionFromPoint(mouseUpPoint, true);

                TextRange fullText = new TextRange(textBlock.ContentStart, textBlock.ContentEnd);

                if (_startSelectPosition == _endSelectPosition && e.ChangedButton == MouseButton.Left)
                {
                    fullText.ApplyPropertyValue(TextElement.BackgroundProperty, textBlock.Background);
                }
                else
                {
                    TextRange selectedText = new TextRange(_startSelectPosition, _endSelectPosition);

                    if (selectedText.Text != string.Empty || e.ChangedButton == System.Windows.Input.MouseButton.Left)
                    {
                        fullText.ApplyPropertyValue(TextElement.BackgroundProperty, textBlock.Background);

                        selectedText.ApplyPropertyValue(TextElement.BackgroundProperty, GetSelectedTextColor(textBlock)/*(SolidColorBrush)new BrushConverter().ConvertFrom("#7CCCDC")*/);

                        _selectedText = selectedText.Text;
                    }
                }
            }
        }
    }
}
