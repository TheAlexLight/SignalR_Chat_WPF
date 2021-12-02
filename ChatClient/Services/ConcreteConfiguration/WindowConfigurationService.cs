using ChatClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.Services
{
    public class WindowConfigurationService : IWindowConfigurationService
    {
        public Window SetWindowStartupData(Window window, double left, double top, double width, double height)
        {
            window.Left = left;
            window.Top = top;
            window.Width = width;
            window.Height = height;

            return window;
        }

        public Window SetWindowStartupData(Window window, double left, double top, double width, double height, 
                double minWidth, double minHeight)
        {
            window.Left = left;
            window.Top = top;
            window.Width = width;
            window.Height = height;
            window.MinWidth = minWidth;
            window.MinHeight = minHeight;

            return window;
        }

        public Window SetWindowStartupData(Window window, double width, double height)
        {
            window.Width = width;
            window.Height = height;

            return window;
        }
    }
}
