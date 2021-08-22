using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ChatClient.Commands
{
    public abstract class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public virtual bool CanExecute(object parameter) => true;

        public abstract void Execute(object parameter);

        public void RaiseCanExecuteChanged()
        {
            Action action = delegate ()
            {
                CanExecuteChanged?.Invoke(this, new EventArgs());
            };

            Application.Current.Dispatcher.Invoke(action, DispatcherPriority.ContextIdle);
        }
    }
}
