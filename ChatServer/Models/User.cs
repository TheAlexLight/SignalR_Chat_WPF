using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Models
{
    public class User : IdentityUser
    {
        public DateTime JoinDate { get; set; }
    }
}
