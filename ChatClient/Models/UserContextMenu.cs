using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatClient.Models
{
    public class UserContextMenu
    {
        public ObservableCollection<UserContextMenu> MenuItems { get; set; }

        public ICommand Command { get; set; }

        public string Header { get; set; }
        public int Role { get; set; }
    }
}
