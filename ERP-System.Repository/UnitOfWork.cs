using ERP_System.Core.Repositories;
using ERP_System.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_System.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;
        private Hashtable _repository;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            _repository = new Hashtable();
        }
        public async Task<int> CompleteAsync() =>  await dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync() => await dbContext.DisposeAsync();

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T).Name;
            if (!_repository.ContainsKey(type))
            {
                var Repository = new GenericRepository<T>(dbContext);
                _repository.Add(type, Repository);
            }
            return _repository[type] as IGenericRepository<T>;
        }
    }
}
