using CefSharp.Wpf;
using ChatClient.Extensions;
using ChatClient.Helpers;
using ChatClient.Interfaces;
using ChatClient.Services;
using ChatClient.ViewModels;
using SharedItems.Enums;
using SharedItems.Models;
using SharedItems.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace ChatClient.Commands
{
    public class SendChatMessageCommand : CommandBase
    {
        private readonly ChatViewModel _viewModel;
        private readonly ISignalRChatService _chatService;
        private readonly Regex _regexUrl = new Regex(@"(?#Protocol)(?:(?:ht|f)tp(?:s?)\:\/\/|~/|/)?(?#Username:Password)(?:\w+:\w+@)?(?#Subdomains)(?:(?:[-\w]+\.)+(?#TopLevel Domains)(?:com|org|net|gov|mil|biz|info|mobi|name|aero|jobs|museum|travel|[a-z]{2}))(?#Port)(?::[\d]{1,5})?(?#Directories)(?:(?:(?:/(?:[-\w~!$+|.,=]|%[a-f\d]{2})+)+|/)+|\?|#)?(?#Query)(?:(?:\?(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)(?:&(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)*)*(?#Anchor)(?:#(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)?");

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
                HyperlinkDescriptionModel hyperlinkDescription = new();

                string message = _viewModel.Message.TextMessage;

                _viewModel.Message.TextMessage = "";

                switch (messageInformationType)
                {
                    case MessageInformationType.Text:
                        messageInformationModel.TextMessage = message.Trim();

                        if (messageInformationModel.TextMessage == string.Empty)
                        {
                            return;
                        }

                        hyperlinkDescription = await FindHyperlinkDescription(messageInformationModel.TextMessage);

                        break;
                    case MessageInformationType.Image:
                        byte[] imageInBytes;

                        if (values.Length == 3)
                        {
                            imageInBytes = File.ReadAllBytes((string)values[2]);
                        }
                        else
                        {
                            imageInBytes = OpenImageFile(); ;
                        }
                        

                        if (imageInBytes != null)
                        {
                            messageInformationModel.ImageMessage = imageInBytes;
                        }
                        else
                        {
                            return;
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
                        ChatGroupModel = _viewModel.CurrentChatGroup.CurrentChatGroupModel,
                        ChatGroupModelId = _viewModel.CurrentChatGroup.CurrentChatGroupModel.Id,
                        CheckStatus = MessageStatus.Received,
                        MessageInformationType = messageInformationType,
                        HyperlinkDescriptionModel = hyperlinkDescription
                    };

                    if (_viewModel.CurrentChatGroup.CurrentChatGroupModel.Name == ChatType.Private)
                    {
                        if (values[1] is UserModel selectedUser)
                        {
                            await _chatService.SendMessage(model, _viewModel.CurrentChatGroup.CurrentChatGroupModel, selectedUser, _viewModel.CurrentUser);
                        }
                    }
                    else
                    {
                        await _chatService.SendMessage(model, _viewModel.CurrentChatGroup.CurrentChatGroupModel, null, _viewModel.CurrentUser);
                    }

                    var scrollViewerExtension = UIHelper.FindChild<ScrollViewerExtension>(Application.Current.MainWindow, "scroll");

                    scrollViewerExtension.RaiseScrollDownEvent();

                    _viewModel.ErrorMessage = string.Empty; 
                }
                catch (Exception)
                {
                    _viewModel.ErrorMessage = "Unable to send message.";
                    _viewModel.Message.TextMessage = "";
                }

                _viewModel.NeedToUpdateMessagesCount = true;
            }
        }

        private async Task<HyperlinkDescriptionModel> FindHyperlinkDescription(string message)
        {
            Match lastRegex = null;

            if (_regexUrl.Matches(message).Count != 0)
            {
                lastRegex = _regexUrl.Matches(message).Last();
            }

            HyperlinkDescriptionModel hyperlinkDescription = new();

            if (lastRegex != null)
            {
                hyperlinkDescription = await HyperlinksDetetectionHelper.ReceivePageSource(lastRegex);
            }

            return hyperlinkDescription;
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
