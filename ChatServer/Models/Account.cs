using ChatServer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Models
{
    public class Account
    {
        public static List<UserHandler> Users { get; set; } = new();
    }
}
