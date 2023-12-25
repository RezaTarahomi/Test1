using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Framework
{
    public interface IDataContext :IScop 

    {        
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        IQueryable<T> DbSet<T>() where T : class;
        void AddRange<T>(params T[] entities) where T : class;
        void DeleteRange<T>(params T[] entities) where T : class;
    }
}
