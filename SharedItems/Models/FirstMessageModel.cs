using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedItems.Models
{
    public static class FirstMessageModel
    {
        public static bool IsFirstMessage { get; set; }
        public static string LastUsername { get; set; }

        public static bool CheckMessage(string currentUsername)
        {
            if (!string.Equals(LastUsername, currentUsername, StringComparison.Ordinal))
            {
                IsFirstMessage = true;
            }
            else
            {
                IsFirstMessage = false;
            }

            LastUsername = currentUsername;

            return IsFirstMessage;
        }
    }
}
