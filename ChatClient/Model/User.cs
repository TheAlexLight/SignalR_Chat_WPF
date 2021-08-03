using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Model
{
    public class User : BaseObject
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
    }
}
