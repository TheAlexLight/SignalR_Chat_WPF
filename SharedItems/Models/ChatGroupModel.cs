using SharedItems.Enums;
using SharedItems.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedItems.Models
{
    public class ChatGroupModel
    {
        public ChatGroupModel()
        {
            Users = new List<UserModel>();
            Messages = new List<MessageModel>();
        }

        public int Id { get; set; }
        public ChatType Name { get; set; }
        public virtual ICollection<UserModel> Users { get; set; }
        
        public virtual ICollection<MessageModel> Messages { get; set; }
    }
}
