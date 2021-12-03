using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Interfaces.SignalRTransmitting
{
    public interface ICredential
    {
        public Task ChangeUsername(UserModel user, string username, string password);
        public Task ChangeEmail(UserModel user, string email, string password);
        public Task ChangePassword(UserModel user, string newPassword, string currentPassword);
    }
}
