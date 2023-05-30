using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveManagement.Web.Contracts;
using LeaveManagement.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Web.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        internal DbSet<T> dbSet;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            this.dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            //await _context.SaveChangesAsync();
            //return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            dbSet.Remove(entity);
            //await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await GetAsync(id);
            return entity != null;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<T> GetAsync(int? id)
        {
             if (id == null)
            {
                return null;
            }
            return await dbSet.FindAsync(id);
        }

        public void UpdateAsync(T entity)
        {
            _context.Update(entity);
            //await _context.SaveChangesAsync();
        }
    }
}