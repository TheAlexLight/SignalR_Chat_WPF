using ChatClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.Supplements.Extensions
{
    public class DropFilesBehaviorExtension
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled", typeof(bool), typeof(DropFilesBehaviorExtension), new FrameworkPropertyMetadata(default(bool), OnPropChanged)
        {
            BindsTwoWayByDefault = false,
        });

        private static void OnPropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FrameworkElement frameworkElement)
            {
                throw new InvalidOperationException();
            }

            if ((bool)e.NewValue)
            {
                frameworkElement.AllowDrop = true;
                frameworkElement.Drop += OnDrop;
                frameworkElement.PreviewDragOver += OnPreviewDragOver;
            }
            else
            {
                frameworkElement.AllowDrop = false;
                frameworkElement.Drop -= OnDrop;
                frameworkElement.PreviewDragOver -= OnPreviewDragOver;
            }
        }

        private static void OnPreviewDragOver(object sender, DragEventArgs e)
        {
            // NOTE: PreviewDragOver subscription is required at least when FrameworkElement is a TextBox
            // because it appears that TextBox by default prevent Drag on preview...
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        private static void OnDrop(object sender, DragEventArgs e)
        {
            object dataContext = ((FrameworkElement)sender).DataContext;
            
            if (dataContext is not IFilesDropped filesDropped)
            {
                if (dataContext != null)
                {
                    Trace.TraceError($"Binding error, '{dataContext.GetType().Name}' doesn't implement '{nameof(IFilesDropped)}'.");
                }

                return;
            }

            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }

            if (e.Data.GetData(DataFormats.FileDrop) is string[] files)
            {
                filesDropped.OnFilesDropped(files);
            }
        }

        public static void SetIsEnabled(DependencyObject element, bool value)
        {
            element.SetValue(IsEnabledProperty, value);
        }

        public static bool GetIsEnabled(DependencyObject element)
        {
            return (bool)element.GetValue(IsEnabledProperty);
        }
    }
}
