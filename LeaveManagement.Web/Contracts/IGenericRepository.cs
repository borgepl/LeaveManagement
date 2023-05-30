using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Web.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetAsync(int? id);
        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task<bool> Exists(int id);
        Task DeleteAsync(int id);
        void UpdateAsync(T entity);
    }
}