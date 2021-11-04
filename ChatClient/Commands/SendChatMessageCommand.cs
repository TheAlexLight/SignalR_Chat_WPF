using ChatClient.Helpers;
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
    public class SendChatMessageCommand : CommandBase
    {
        private readonly ChatViewModel _viewModel;
        private readonly ISignalRChatService _chatService;

        public SendChatMessageCommand(ChatViewModel viewModel, ISignalRChatService chatService)
        {
            _viewModel = viewModel;
            _chatService = chatService;
        }

        public override async void Execute(object parameter)
        {
            object[] values = parameter as object[];

            if (values[0] is MessageInformationType messageInformationType)
            {
                MessageInformationModel messageInformationModel = new();

                switch (messageInformationType)
                {
                    case MessageInformationType.Text:
                        messageInformationModel.TextMessage = _viewModel.Message.TextMessage.Trim();
                        break;
                    case MessageInformationType.Image:
                        byte[] imageInBytes = OpenImageFile();

                        if (imageInBytes != null)
                        {
                            messageInformationModel.ImageMessage = imageInBytes;
                        }

                        break;
                }

                try
                {
                    MessageModel model = new MessageModel()
                    {
                        Sender = _viewModel.CurrentUser.UserProfile.Username,
                        Message = messageInformationModel,
                        Time = DateTime.Now,
                        UserModelId = _viewModel.CurrentUser.Id,
                        ChatGroupModel = _viewModel.CurrentChatGroup,
                        ChatGroupModelId = _viewModel.CurrentChatGroup.Id,
                        CheckStatus = MessageStatus.Received,
                        MessageInformationType = messageInformationType
                    };

                    if (_viewModel.CurrentChatGroup.Name == ChatType.Private)
                    {
                        if (values[1] is UserModel selectedUser)
                        {
                            await _chatService.SendMessage(model, _viewModel.CurrentChatGroup, selectedUser, _viewModel.CurrentUser);
                        }
                    }
                    else
                    {
                        await _chatService.SendMessage(model, _viewModel.CurrentChatGroup, null, _viewModel.CurrentUser);
                    }

                    _viewModel.ErrorMessage = string.Empty;
                    _viewModel.Message.TextMessage = "";
                }
                catch (Exception)
                {
                    _viewModel.ErrorMessage = "Unable to send message.";
                    _viewModel.Message.TextMessage = "";
                }

                _viewModel.NeedToUpdateMessagesCount = true;
            }
        }

        private byte[] OpenImageFile()
        {
            SelectFileHelper helper = new();

            string filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)" +
                    "   |*.jpg; *.jpeg; *.gif; *.bmp; *.png";

            byte[] imageInBytes = helper.SelectFileInBytes(filter);

            return imageInBytes;
        }
    }
}
