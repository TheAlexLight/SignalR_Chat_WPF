using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharedItems.Models
{
    public class UserProfileModel
    {
        public UserProfileModel()
        {
            //StatusMessage = "Active";

            //Image = File.ReadAllBytes("Resources/Default/defaultUser.png");
            UsernameColor = "#1E90FF";
            Role = "User";
        }

        public int Id { get; set; }
        public int UserModelId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string StatusMessage { get; set; }
        public byte[] Image { get; set; }
        public string UsernameColor { get; set; }
        public bool IsOnline { get; set; }
        public string Role { get; set; }
    }
}
