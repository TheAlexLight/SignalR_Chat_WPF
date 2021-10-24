using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.Commands
{
    class MessageLoadedCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            object[] values = parameter as object[];

            if (values[0] is MessageModel message)
            {
                Thickness margin = new Thickness(int.Parse((string)values[2]));
                FrameworkElement control = (FrameworkElement)values[1];
                message.MessageHeight = control.ActualHeight + margin.Top + margin.Bottom;
            }
        }
    }
}
