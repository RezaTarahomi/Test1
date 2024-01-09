using Application.Entities;
using Application.Framework;
using Database.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Entities
{
    public class EntityRepository : IEntityRepository
    {
        private readonly IDataContext db;
        private readonly IQueryable<Entity> entities;
        private readonly IQueryable<Field> fields;



        public EntityRepository(IDataContext db)
        {
            this.db = db;
            entities = db.DbSet<Entity>();
            fields = db.DbSet<Field>();
        }

        public async Task<List<Entity>> GetAll()
        {
            return await entities
                .Include(x=>x.Fields)
                .ThenInclude(x=>x.EnumType)
                .ThenInclude(x=>x.EnumFields)                
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Field>> GetAllFields()
        {
            return await fields.ToListAsync();
        }

        public async Task<Entity?> GetById(int id)
        {
            return await entities.AsNoTracking()
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsExist(int id)
        {
            return await entities.AsNoTracking()
                .AnyAsync(e => e.Id == id);
        }

        public void Add(Entity entity)
        {
              db.Add(entity);
        }

        public void Update(Entity entity)
        {
            db.Update(entity);
        }

        public void Delete(Entity entity)
        {
            db.Delete(entity);
        }

        public void AddRangeFields(List<Field> fields)
        {
            db.AddRange(fields);           
        }

        public void DeleteRangeFields(List<Field> fields)
        {

            db.DeleteRange(fields);
        }

       
    }
}
