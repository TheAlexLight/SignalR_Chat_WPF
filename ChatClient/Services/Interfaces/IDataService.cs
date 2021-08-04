using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Services.Interfaces
{
    public interface IDataService<T>
    {
        Task<T> Create(T entity);
        Task<bool> Remove(int id);
    }
}
