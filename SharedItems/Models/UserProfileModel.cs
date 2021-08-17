using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedItems.Models
{
    public class UserProfileModel
    {
        public UserProfileModel()
        {
            StatusMessage = "Active";
            UsernameColor = "#1E90FF";
        }

        public string Username { get; set; }
        public string StatusMessage { get; set; }
        public string ImagePath { get; set; }
        public string UsernameColor { get; set; }
    }
}
