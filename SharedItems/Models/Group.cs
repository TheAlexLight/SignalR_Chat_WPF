using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedItems.Models
{
    public class Group
    {
        public Group()
        {
            IsChecked = true;
        }
        public string Name { get; set; }
        public ObservableCollection<UserModel> GroupedUsers { get; set; }
        public bool IsChecked { get; set; }
        public int UsersCount {
            get
            {
                if (GroupedUsers != null)
                {
                    return GroupedUsers.Count;
                }

                return 0;
            }
        }
    }
}
