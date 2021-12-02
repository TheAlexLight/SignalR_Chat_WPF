using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.Supplements.Helpers
{
    public class SelectFileHelper
    {
        public byte[] SelectFileInBytes(string filter)
        {
            byte[] imageIntoBytes = null;

            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();

                fileDialog.Filter = filter;

                if (fileDialog.ShowDialog() == true)
                {
                     imageIntoBytes = File.ReadAllBytes(fileDialog.FileName);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("An error occured: {0}", e.Message));
            }

            return imageIntoBytes;
        }
    }
}
