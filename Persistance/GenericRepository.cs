using Application.Framework;
using Database.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance
{
    public class DataContext : IDataContext
    {
        private readonly CodeGeneratorDbContext _dbContext;
        

        public DataContext(CodeGeneratorDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add<T>(T entity) where T : class
        {
            _dbContext.Add(entity);
        }

        public void AddRange<T>(params T[] entities) where T : class
        {
            _dbContext.AddRange(entities);
        }

        public IQueryable<T> DbSet<T>() where T : class
        {
            return _dbContext.Set<T>();
        }

        public void Delete<T>(T entity) where T : class
        {
            _dbContext.Remove(entity);
        }

        public void DeleteRange<T>(params T[] entities) where T : class
        {           
            _dbContext.RemoveRange(entities);           
        }

        public void Update<T>(T entity) where T : class
        {
            _dbContext.Update(entity);
        }

        
    }
}
