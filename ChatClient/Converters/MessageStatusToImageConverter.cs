using SharedItems.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ChatClient.Converters
{
    public class MessageStatusToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;

            if (value is MessageStatus messageStatus)
            {
                switch (messageStatus)
                {
                    case MessageStatus.None:
                        break;
                    case MessageStatus.Sent:
                        break;
                    default:
                        break;
                    case MessageStatus.Received:
                        result = "uncheckedDrawingImage";
                        break;
                    case MessageStatus.Read:
                        result = "checkedDrawingImage";
                        break;
                }
            }

            return result != string.Empty ? Application.Current.FindResource(result) : result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
