using SharedItems.Enums;
using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ChatClient.Converters
{
    public class UncheckedMessagesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var a = value as UserModel;
            ChatGroupModel b = a.Groups.FirstOrDefault(g => g.Name == ChatType.Private);
            int c = b.Messages.Where(m => m.Sender != a.UserProfile.Username && m.CheckStatus != MessageStatus.Read).Count();

            return c.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
