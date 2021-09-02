using ChatClient.Services;
using ChatClient.ViewModels;
using SharedItems.Models;
using SharedItems.Models.StatusModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Commands.ContextMenuCommands
{
    public class BanUserCommand : CommandBase
    {
        private readonly ChatViewModel _viewModel;

        public BanUserCommand(ChatViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override async void Execute(object parameter)
        {
            var values = (object[])parameter;
            var duration = (string)values[1];

            if (values[0] is UserProfileModel)
            {
                UserProfileModel user = values[0] as UserProfileModel;

                if (user.Username != _viewModel.CurrentUser.Username)
                {
                    BanStatusModel model = new BanStatusModel();

                    if (duration.Equals("Permanent"))
                    {
                        model.IsPermanent = true;
                    }
                    else
                    {
                        model.Duration = Int32.Parse(duration);

                        model.EndTime = DateTime.Now;
                        model.EndTime.AddMinutes(model.Duration);
                    }

                    model.IsBanned = true;

                    await _viewModel.ChatService.SendBan(user.Username, model);
                }
            }
        }
    }
}
