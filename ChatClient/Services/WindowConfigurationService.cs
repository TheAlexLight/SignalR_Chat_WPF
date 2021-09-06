using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.Services
{
    public class WindowConfigurationService
    {
        private readonly Window _window;

        public WindowConfigurationService(Window window)
        {
           _window = window;
        }

        public Window SetWindowStartupData(double left, double top, double width, double height)
        {
            _window.Left = left;
            _window.Top = top;
            _window.Width = width;
            _window.Height = height;

            return _window;
        }
    }
}
