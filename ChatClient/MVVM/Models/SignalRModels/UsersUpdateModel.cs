using ChatClient.Interfaces.SignalRTransmitting;
using ChatClient.Interfaces.SignalRTransmitting.SignalTranmittingResult;
using Microsoft.AspNetCore.SignalR.Client;
using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.MVVM.Models.SignalRModels
{
    public class UsersUpdateModel : IUsersUpdateResult
    {
        private readonly IEventHandler _chatService;

        public UsersUpdateModel(IEventHandler chatService)
        {
            _chatService = chatService;

            _chatService.Connection.On<UserModel>("ReceiveCurrentUser", (currentUser) => _currentUserReceived?.Invoke(currentUser));
            _chatService.Connection.On<ObservableCollection<UserModel>>("ReceiveUserList", (users) => _userListReceived?.Invoke(users));
            _chatService.Connection.On<ChatGroupModel>("ReceiveCurrentGroup", (group) => _currentGroupReceived?.Invoke(group));
        }

        private Action<UserModel> _currentUserReceived;
        private Action<ObservableCollection<UserModel>> _userListReceived;
        private Action<ChatGroupModel> _currentGroupReceived;

        public event Action<UserModel> CurrentUserReceived
        {
            add
            {
                if (!_chatService.IsEventHandlerRegistered(_currentUserReceived, value))
                {
                    _currentUserReceived += value;
                }
            }

            remove => _currentUserReceived -= value;
        }

        public event Action<ObservableCollection<UserModel>> UserListReceived
        {
            add
            {
                if (!_chatService.IsEventHandlerRegistered(_userListReceived, value))
                {
                    _userListReceived += value;
                }
            }

            remove => _userListReceived -= value;
        }

        public event Action<ChatGroupModel> CurrentGroupReceived
        {
            add
            {
                if (!_chatService.IsEventHandlerRegistered(_currentGroupReceived, value))
                {
                    _currentGroupReceived += value;
                }
            }

            remove => _currentGroupReceived -= value;
        }
    }
}
