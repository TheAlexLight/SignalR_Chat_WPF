using ChatClient.ViewModels;
using SharedItems.Enums;
using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ChatClient.Converters
{
    public class UncheckedMessagesConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;

            UserModel currentUser = values[1] as UserModel;
            UserModel selectedUser = values[0] as UserModel;

            if (selectedUser.Groups != null 
                    && currentUser != null
                    && selectedUser != null)
            {
                var e = selectedUser.Groups.FirstOrDefault(u => u.Name == ChatType.Private
                && u.Users.Any(u => u.UserProfile.Username == currentUser.UserProfile.Username));

                if (e != null)
                {
                    int count = e.Messages.Where(m => m.Sender != currentUser.UserProfile.Username
                        && m.CheckStatus != MessageStatus.Read).Count();

                    if (count != 0)
                    {
                        result = count.ToString();
                    }
                }
            }

            return result;
            //return c.ToString();
        }

        public object[] ConvertBack(object values, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
