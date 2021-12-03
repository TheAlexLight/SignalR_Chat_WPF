using ChatClient.Interfaces.SignalRTransmitting;
using ChatClient.Interfaces.SignalRTransmitting.SignalTranmittingResult;
using Microsoft.AspNetCore.SignalR.Client;
using SharedItems.Models.StatusModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.MVVM.Models.SignalRModels
{
    public class AdminActionModel : IAdminActionResult
    {
        private readonly IEventHandler _chatService;

        public AdminActionModel(IEventHandler chatService)
        {
            _chatService = chatService;

            _chatService.Connection.On<BanStatusModel>("ReceiveBan", (model) => _banResultReceived?.Invoke(model));            
            _chatService.Connection.On("ReceiveKick", () => _kickResultReceived?.Invoke());
            _chatService.Connection.On<MuteStatusModel>("ReceiveMute", (model) => _muteResultReceived?.Invoke(model));
        }

        private Action<BanStatusModel> _banResultReceived;
        private Action _kickResultReceived;
        private Action<MuteStatusModel> _muteResultReceived;

        public event Action<BanStatusModel> BanResultReceived
        {
            add
            {
                if (!_chatService.IsEventHandlerRegistered(_banResultReceived, value))
                {
                    _banResultReceived += value;
                }
            }

            remove => _banResultReceived -= value;
        }

        public event Action KickResultReceived
        {
            add
            {
                if (!_chatService.IsEventHandlerRegistered(_kickResultReceived, value))
                {
                    _kickResultReceived += value;
                }
            }

            remove => _kickResultReceived -= value;
        }

        public event Action<MuteStatusModel> MuteResultReceived
        {
            add
            {
                if (!_chatService.IsEventHandlerRegistered(_muteResultReceived, value))
                {
                    _muteResultReceived += value;
                }
            }

            remove => _muteResultReceived -= value;
        }

        public async Task Ban(string username, BanStatusModel model)
        {
            await _chatService.Connection.SendAsync("SendUserBan", username, model);
        }

        public async Task Mute(string username, MuteStatusModel model)
        {
            await _chatService.Connection.SendAsync("SendUserMute", username, model);
        }

        public async Task Kick(string username)
        {
            await _chatService.Connection.SendAsync("SendKickUser", username);
        }
    }
}
