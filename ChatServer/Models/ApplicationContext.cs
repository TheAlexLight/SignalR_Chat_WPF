using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedItems.Models;
using SharedItems.Models.StatusModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<MessageModel> Messages { get; set; }
        public DbSet<UserProfileModel> UserProfiles { get; set; }
        public DbSet<UserModel> UserModels { get; set; }
        public DbSet<UserStatusModel> UsersStatus { get; set; }
        public DbSet<BanStatusModel> BansStatus { get; set; }
        public DbSet<MuteStatusModel> MutesStatus { get; set; }
        public DbSet<ChatGroupModel> Groups { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.Migrate();
            //Database.EnsureDeleted();
        }
    }
}
