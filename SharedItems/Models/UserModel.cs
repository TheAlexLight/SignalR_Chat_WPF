using SharedItems.Models.StatusModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedItems.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        //public int PrivateChatId { get; set; }
        public UserProfileModel UserProfile { get; set; }
        public UserStatusModel UserStatus { get; set; }
        public ICollection<MessageModel> Messages { get; set; }
    }
}
