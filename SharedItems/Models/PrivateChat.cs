using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedItems.Models
{
    public class PrivateChat
    {
        public int Id { get; set; }
        public UserModel FirstUser { get; set; }
        public UserModel SecondUser { get; set; }
        public ICollection<MessageModel> Messages { get; set; }
    }
}
