using ChatClient.Services.Interfaces;
using EntityFramework.Models;
using System.Threading.Tasks;

namespace ChatClient.Services
{
    public interface IAccountService<T> : IDataService<T>
    {
        Task<T> GetByUsername(string username);
        Task<T> GetByEmail(string email);
    }
}