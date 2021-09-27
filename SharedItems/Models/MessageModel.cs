using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedItems.Models
{
    public class MessageModel
    {
        public MessageModel()
        {
            GroupName = "mainChat";
        }

        public int Id { get; set; }
        public int UserModelId { get; set; }
        //public int PrivateChatId { get; set; }
        public DateTime Time { get; set; }
        public bool IsFirstMessage { get; set; }
        public string Message { get; set; }
        public string GroupName { get; set; }

        public UserModel UserModel { get; set; }
        //public PrivateChat PrivateChat { get; set; }
    }
}
