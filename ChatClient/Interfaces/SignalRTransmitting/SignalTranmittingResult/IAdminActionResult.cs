using SharedItems.Models.StatusModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Interfaces.SignalRTransmitting.SignalTranmittingResult
{
    public interface IAdminActionResult : IAdminAction
    {
        public event Action<BanStatusModel> BanResultReceived;
        public event Action KickResultReceived;
        public event Action<MuteStatusModel> MuteResultReceived;
    }
}
