using ChatClient.Interfaces.BaseConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Interfaces.SignalRTransmitting
{
    public interface IEventHandler : ISignalRService
    {
        public bool IsEventHandlerRegistered(Delegate handlerList, Delegate prospectiveHandler);
    }
}
