using ERP_System.Core.Repositories;
using ERP_System.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_System.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddAsync(T item)
        {
            await dbContext.Set<T>().AddAsync(item);
        }

        public void DeleteAsync(T item)
        {
             dbContext.Set<T>().Remove(item);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public Task UpdateAsync(T item)
        {
            throw new NotImplementedException();
        }
    }
}
