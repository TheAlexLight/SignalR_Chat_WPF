using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedItems.Models
{
    public class PrivateChat
    {
        public UserModel FirstUser { get; set; }
        public UserModel SecondUser { get; set; }
        public MessageModel Messages { get; set; }
    }
}
