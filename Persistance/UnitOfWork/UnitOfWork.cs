using Application.Framework;
using Database.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CodeGeneratorDbContext dbContext;

        public UnitOfWork(CodeGeneratorDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task BeginTransaction()
        {
           await dbContext.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
          await  dbContext.Database.CommitTransactionAsync();
        }

        public async Task RoleBack()
        {
            await dbContext.Database.RollbackTransactionAsync();
        }

        public async Task SaveChanges()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
