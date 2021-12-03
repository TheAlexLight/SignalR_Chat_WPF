using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Interfaces.SignalRTransmitting.SignalTranmittingResult
{
    public interface IAuthorizationResult : IAuthorization
    {
        public event Action<bool, string> RegistrationResultReceived;
        public event Action<bool> LoginResultReceived;
    }
}
