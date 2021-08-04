using ChatClient.Services.Interfaces;
using EntityFramework.DbContexts;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Services
{
    public class AccountDataService<T>  : IAccountService<T>
        where T : BaseObject
    {
        public async Task<T> Create(T entity)
        {
            using (AuthorizationDbContext context = new AuthorizationDbContext())
            {
                EntityEntry<T> createdResult = await context.AddAsync(entity);
                await context.SaveChangesAsync();

                return createdResult.Entity;
            }
        }

        public async Task<bool> Remove(int id)
        {
            using (AuthorizationDbContext context = new AuthorizationDbContext())
            {
                T entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.Id == id);
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();

                return true;
            }
        }

        public async Task<Account> GetByEmail(string email)
        {
            using (AuthorizationDbContext context = new AuthorizationDbContext())
            {
                return await context.Accounts
                    .FirstOrDefaultAsync(a => a.AccountHolder.Email == email);
            }
        }

        public async Task<Account> GetByUsername(string username)
        {
            using (AuthorizationDbContext context = new AuthorizationDbContext())
            {
                return await context.Accounts
                    .Include(a=>a.AccountHolder)
                    .FirstOrDefaultAsync(a => a.AccountHolder.Username == username);
            }
        }
    }
}
