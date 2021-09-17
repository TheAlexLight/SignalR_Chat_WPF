using Microsoft.AspNetCore.Identity;
using SharedItems.Models;
using SharedItems.Models.StatusModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            JoinDate = DateTime.Now;
        }

        public DateTime JoinDate { get; set; }
        public UserModel UserModel { get; set; }
        //public UserStatusModel UserStatus { get; set; }
    }
}
