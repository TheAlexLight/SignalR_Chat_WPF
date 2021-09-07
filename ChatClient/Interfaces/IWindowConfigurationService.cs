using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.Interfaces
{
    public interface IWindowConfigurationService
    {
        public Window SetWindowStartupData(Window window, double width, double height);
        public Window SetWindowStartupData(Window window, double left, double top, double width, double height);
        public Window SetWindowStartupData(Window window, double left, double top, double width, double height,
                double minWidth, double minHeight);
    }
}
