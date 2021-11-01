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
            var a = values[0] as UserModel;
            var d = values[1] as UserModel;
            //var e = a.Groups.FirstOrDefault(u => u.Name == ChatType.Private 
            //    && u.Users.Any(u=>u.UserProfile.Username == d.UserProfile.Username));
            ////ChatGroupModel b = a.Groups.FirstOrDefault(g => g.Name == ChatType.Private);
            ////int c = b.Messages.Where(m => m.Sender != a.UserProfile.Username && m.CheckStatus != MessageStatus.Read).Count();

            string result = string.Empty;

            //if (e != null)
            //{
            //    int count = e.Messages.Where(m => m.Sender != d.UserProfile.Username
            //        && m.CheckStatus != MessageStatus.Read).Count();

            //    if (count != 0)
            //    {
            //        result = count.ToString();
            //    }

            //    //result = e.Messages.Count.ToString();
            //}

            return result;
            //return c.ToString();
        }

        public object[] ConvertBack(object values, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
