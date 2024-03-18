using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedLibrary.Interfaces
{
    public interface IDBService
    {
        Task<List<T>> GetAllAsync<T>();
        Task<T> GetByIdAsync<T>(int id);
        Task<T> CreateAsync<T>(T entity);
        Task<T> UpdateAsync<T>(T entity);
        Task<T> DeleteAsync<T>(int id);
    }
}