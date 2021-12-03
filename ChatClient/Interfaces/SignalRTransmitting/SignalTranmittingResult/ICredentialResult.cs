using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Interfaces.SignalRTransmitting.SignalTranmittingResult
{
    public interface ICredentialResult : ICredential
    {
        public event Func<bool, string, string, Task> UserCredentialsResultReceived;
    }
}
