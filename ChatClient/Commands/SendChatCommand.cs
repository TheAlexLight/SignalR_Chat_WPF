using ChatClient.Interfaces;
using ChatClient.Services;
using ChatClient.ViewModels;
using SharedItems.Enums;
using SharedItems.Models;
using SharedItems.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatClient.Commands
{
    public class SendChatCommand : CommandBase
    {
        private readonly ChatViewModel _viewModel;
        private readonly ISignalRChatService _chatService;

        public SendChatCommand(ChatViewModel viewModel, ISignalRChatService chatService)
        {
            _viewModel = viewModel;
            _chatService = chatService;
        }

        public override async void Execute(object parameter)
        {
            try
            {
                MessageViewModel model = new MessageViewModel()
                {
                    Message = _viewModel.Message,
                    Time = DateTime.Now,
                    UserModel = _viewModel.CurrentUser,
                    UserModelId = _viewModel.CurrentUser.Id,
                    ChatGroupModel = _viewModel.CurrentChatGroup,
                    ChatGroupModelId = _viewModel.CurrentChatGroup.Id,
                    CheckStatus = MessageStatus.Received
                };

                if (_viewModel.CurrentChatGroup.Name == ChatType.Private)
                {
                    if (parameter is UserModel selectedUser)
                    {
                        await _chatService.SendMessage(model, _viewModel.CurrentChatGroup, selectedUser, _viewModel.CurrentUser);
                    }
                }
                else
                {
                    await _chatService.SendMessage(model, _viewModel.CurrentChatGroup, null, _viewModel.CurrentUser);
                }

                

                _viewModel.ErrorMessage = string.Empty;
                _viewModel.Message = "";
            }
            catch (Exception)
            {
                _viewModel.ErrorMessage = "Unable to send message.";
                _viewModel.Message = "";
            }
        }
    }
}
