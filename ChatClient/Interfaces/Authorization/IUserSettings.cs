using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Interfaces.Authorization
{
    public interface IUserSettings : IRegistration
    {
        public string UsernameSettings { get; set; }
        public string EmailSettings { get; set; }
    }
}
