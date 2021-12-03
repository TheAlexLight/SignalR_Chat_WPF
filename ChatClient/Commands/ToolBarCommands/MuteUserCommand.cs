using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.MVVM.ViewModels.ChatMainViewModels;
using SharedItems.Models;
using SharedItems.Models.StatusModels;

namespace ChatClient.Commands.ToolBarCommands
{
    public class MuteUserCommand : CommandBase
    {
        private readonly ChatViewModel _viewModel;

        public MuteUserCommand(ChatViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override async void Execute(object parameter)
        {
            var values = (object[])parameter;
            var duration = (string)values[1];

            if (values[0] is UserModel)
            {
                UserModel user = values[0] as UserModel;

                if (user.UserProfile.Username != _viewModel.CurrentUser.UserProfile.Username)
                {
                    MuteStatusModel model = new MuteStatusModel();

                    if (duration.Equals("Permanent"))
                    {
                        model.IsPermanent = true;
                    }
                    else
                    {
                        model.Duration = Int32.Parse(duration);

                        model.EndTime = DateTime.Now;
                        model.EndTime = model.EndTime.AddMinutes(model.Duration);
                    }

                    model.IsMuted = true;

                    await _viewModel.BaseConfiguration.ChatService.AdminActionModel.Mute(user.UserProfile.Username, model);
                }
            }
        }
    }
}
