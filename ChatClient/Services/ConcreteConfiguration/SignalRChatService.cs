using ChatClient.Enums;
using ChatClient.Interfaces;
using ChatClient.Interfaces.BaseConfiguration;
using ChatClient.Interfaces.SignalRTransmitting;
using ChatClient.Interfaces.SignalRTransmitting.SignalTranmittingResult;
using ChatClient.MVVM.Models.SignalRModels;
using MessagePack;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SharedItems.Enums;
using SharedItems.Models;
using SharedItems.Models.AuthenticationModels;
using SharedItems.Models.StatusModels;
using SharedItems.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace ChatClient.Services.ConcreteConfiguration
{
    public class SignalRChatService : ISignalRChatService
    {
        public SignalRChatService(HubConnectionBuilder connectionBuilder)
        {
            Connection = connectionBuilder
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions.MaxDepth = 64;
                    options.PayloadSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                })
                    .WithUrl("http://localhost:5000/chat")
                    .WithAutomaticReconnect()
                    .Build();

            AuthorizationModel = new AuthorizationModel(this);
            CredentialModel = new CredentialModel(this);

            AdminActionModel = new AdminActionModel(this);
            MessagesManagementModel = new MessagesManagementModel(this);

            UsersUpdateModel = new UsersUpdateModel(this);

            MessageContextMenu = new MessageContextMenu(this);
        }

        public HubConnection Connection { get; }

        public IAuthorizationResult AuthorizationModel { get; set; }
        public ICredentialResult CredentialModel { get; set; }

        public IAdminActionResult AdminActionModel { get; set; }
        public IMessagesManagement MessagesManagementModel { get; set; }

        public IUsersUpdateResult UsersUpdateModel { get; set; }

        public IContextMenu MessageContextMenu { get; set; }

        public bool IsEventHandlerRegistered(Delegate handlerList, Delegate prospectiveHandler)
        {
            if (handlerList != null)
            {
                foreach (Delegate existingHandler in handlerList.GetInvocationList())
                {
                    if (existingHandler.Method == prospectiveHandler.Method)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task SwitchChat(ChatType chatTtype, UserModel currentUser)
        {
            await Connection.SendAsync("SendSwitchChat", chatTtype, currentUser);
        }

        public async Task ChangePhoto(UserModel currentUser, byte[] photo)
        {
            await Connection.SendAsync("SendChangePhoto", currentUser, photo);
        }
    }
}
