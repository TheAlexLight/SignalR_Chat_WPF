using SharedItems.Models.StatusModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Interfaces.SignalRTransmitting
{
    public interface IAdminAction
    {
        public Task Ban(string username, BanStatusModel model);
        public Task Mute(string username, MuteStatusModel model);
        public Task Kick(string username);
    }
}
