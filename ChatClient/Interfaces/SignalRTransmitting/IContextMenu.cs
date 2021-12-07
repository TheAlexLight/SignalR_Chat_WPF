using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Interfaces.SignalRTransmitting
{
    public interface IContextMenu
    {
        public Task Delete(MessageModel message);
    }
}
