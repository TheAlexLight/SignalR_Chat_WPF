using ChatClient.Services.Interfaces;
using EntityFramework.Models;
using System.Threading.Tasks;

namespace ChatClient.Services.Interfaces
{
    public interface IAccountService<T> : IDataService<T>
    {
        Task<Account> GetByUsername(string username);
        Task<Account> GetByEmail(string email);
    }
}