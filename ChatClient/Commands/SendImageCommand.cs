using ChatClient.Helpers;
using ChatClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Commands
{
    public class SendImageCommand : CommandBase
    {
        private readonly ChatViewModel _viewModel;

        public SendImageCommand(ChatViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            //string filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)" +
            //        "   |*.jpg; *.jpeg; *.gif; *.bmp; *.png";

            //SelectFileHelper helper = new SelectFileHelper();

            //helper.ChooseFile(filter, _viewModel.ChatService.SendImage, _viewModel);
        }
    }
}
