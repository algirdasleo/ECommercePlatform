using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedLibrary.Interfaces
{
    public interface IDBService<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(int id);
    }
}
