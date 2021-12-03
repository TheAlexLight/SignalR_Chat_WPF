using SharedItems.Models.AuthenticationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Interfaces.SignalRTransmitting
{
    public interface IAuthorization
    {
        public Task Register(UserRegistrationModel model);
        public Task Login(UserLoginModel model);
        public Task Connect();
        public Task Reconnect(string username);
    }
}
