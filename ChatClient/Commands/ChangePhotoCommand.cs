using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using ChatClient.MVVM.ViewModels.ChatMainViewModels;
using ChatClient.Supplements.Helpers;

namespace ChatClient.Commands
{
    public class ChangePhotoCommand : CommandBase
    {
        private readonly ChatViewModel _viewModel;

        public ChangePhotoCommand(ChatViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            string filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)" +
                    "   |*.jpg; *.jpeg; *.gif; *.bmp; *.png";

            SelectFileHelper helper = new SelectFileHelper();

            byte[] imageInBytes = helper.SelectFileInBytes(filter);

            if (imageInBytes != null)
            {
                _viewModel.BaseConfiguration.ChatService.ChangePhoto(_viewModel.CurrentUser, imageInBytes);
            }
        }
    }
}
