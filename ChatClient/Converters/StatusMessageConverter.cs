using ChatClient.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ChatClient.Converters
{
    public class StatusMessageConverter : IValueConverter
    {
        public StatusMessageConverter()
        {
            InitializeFields();
        }

        private IDictionary<ConnectionStatus, string> _connectionStatusMessages;

        private void InitializeFields()
        {
            _connectionStatusMessages = new Dictionary<ConnectionStatus, string>()
            {
                {ConnectionStatus.None, ""},
                {ConnectionStatus.Connected, "Connected"},
                {ConnectionStatus.Connecting, "Connecting"},
                {ConnectionStatus.Disconnected, "Disconnected"},
                {ConnectionStatus.Reconnecting, "Reconnecting"}
            };
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string statusMessage = _connectionStatusMessages.TryGetValue((ConnectionStatus)value, out statusMessage)
                ? _connectionStatusMessages[(ConnectionStatus)value]
                : "Error: Status message wasn't received";

            return statusMessage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
