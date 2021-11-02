using ChatServer.Models;
using Microsoft.EntityFrameworkCore;
using SharedItems.Enums;
using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Helpers
{
    public class GroupsHelper
    {
        public async Task<ChatGroupModel> AddUsersToPrivateGroup(ApplicationContext dbContext, string currentUsername, string selectedUsername)
        {
            ChatGroupModel group = new ChatGroupModel()
            {
                Name = ChatType.Private
            };

            group.Users = new List<UserModel>();

            group.Users.Add(await dbContext.UserModels
                    .FirstOrDefaultAsync(u => u.UserProfile.Username
                        == selectedUsername));
            group.Users.Add(await dbContext.UserModels
                    .FirstOrDefaultAsync(u => u.UserProfile.Username
                        == currentUsername));

            dbContext.Groups.Add(group);
            await dbContext.SaveChangesAsync();

            return group;
        }
    }
}
