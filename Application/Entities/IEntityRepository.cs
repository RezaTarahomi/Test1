using Application.Framework;
using Database.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities
{
    public interface IEntityRepository : IScop
    {
        void Add(Entity entity);
        void AddRangeFields(List<Field> fields);
        void Delete(Entity entity);
        void DeleteRangeFields(List<Field> fields);
        Task<List<Entity>> GetAll();
        Task<List<Field>> GetAllFields();
        Task<Entity?> GetById(int id);
        Task<bool> IsExist(int id);
        void Update(Entity entity);
    }
}
