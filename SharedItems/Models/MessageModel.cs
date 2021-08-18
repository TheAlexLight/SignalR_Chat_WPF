using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedItems.Models
{
    public class MessageModel
    {
        public UserProfileModel UserInfo { get; set; }
        public DateTime Time { get; set; }
        public bool IsFirstMessage { get; set; }
        public string Message { get; set; }
    }
}
