using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Interfaces.Authorization
{
    public interface ILogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
