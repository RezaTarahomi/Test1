using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Framework
{
    public interface IUnitOfWork : IScop
    {
        public Task BeginTransaction();
        public Task Commit();
        public Task RoleBack();
        public Task SaveChanges();
    }
}
