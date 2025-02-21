using ERP_System.Core.Repositories;
using ERP_System.Core.Specifications;
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

        public void Delete(T item)
        {
             dbContext.Set<T>().Remove(item);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public void Update(T item)
        {
            dbContext.Set<T>().Update(item);
        }
        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
        {
            return await SpecificationEvalutor<T>.GetQuery(dbContext.Set<T>(), spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            var data = await SpecificationEvalutor<T>.GetQuery(dbContext.Set<T>(), spec).ToListAsync();
            return data;
        }
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvalutor<T>.GetQuery(dbContext.Set<T>(), spec);
        }
    }
}
