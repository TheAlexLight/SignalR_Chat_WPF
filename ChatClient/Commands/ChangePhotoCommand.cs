using ChatClient.ViewModels;
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
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();

                fileDialog.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)" +
                    "   |*.jpg; *.jpeg; *.gif; *.bmp; *.png";

                if (fileDialog.ShowDialog() == true)
                {
                    //byte[] imageIntoBytes = Encoding.UTF8.GetBytes(fileDialog.FileName);
                    byte[] imageIntoBytes = File.ReadAllBytes(fileDialog.FileName);

                    _viewModel.ChatService.ChangePhoto(_viewModel.CurrentUser, imageIntoBytes);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("An error occured: {0}",e.Message));
            }
        }
    }
}
