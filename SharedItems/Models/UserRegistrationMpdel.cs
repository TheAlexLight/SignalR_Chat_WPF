using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedItems.Models
{
    public class RegistrationUserData
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
