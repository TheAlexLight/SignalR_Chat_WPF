using ChatClient.Supplements.Extensions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ChatClient.Converters
{
    public class ByteToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage image = new BitmapImage();

            if (value is byte[] imageInBytes)
            {
                try
                {
                    Stream imageStream = new MemoryStream(imageInBytes);

                    image.BeginInit();
                    image.StreamSource = imageStream;
                    image.EndInit();            
                }
                catch (Exception ex)
                {
                    image = null;
                    MessageBox.Show(string.Format("Can't load image: {0}", ex.Message));
                }
            }
            else
            {
                image = null;
            }

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
