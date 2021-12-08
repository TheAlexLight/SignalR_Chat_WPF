using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Interfaces.Authorization
{
    public interface IRegistration : ILogin
    {
        public string Email { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
