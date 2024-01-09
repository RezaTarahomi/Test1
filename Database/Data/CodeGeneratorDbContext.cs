using Database.Data.Config;
using Database.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Data
{
    public class CodeGeneratorDbContext : DbContext
    {
        public CodeGeneratorDbContext(DbContextOptions<CodeGeneratorDbContext> options) : base(options)
        {
        }

        public DbSet<Api> Apis { get; set; }
        public DbSet<RequestParameter> RequestParameters { get; set; }
        public DbSet<ResponseParameter> ResponseParameters { get; set; }
        public DbSet<ObjectType> ObjectTypes { get; set; }
        public DbSet<Entity> Entities { get; set; }
        public DbSet<Field> Fields { get; set; }
        // public DbSet<EntityParent> EntityParents { get; set; }
        public DbSet<EnumType> EnumTypes { get; set; }
        public DbSet<EnumField> EnumFields { get; set; }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RequestParameterConfig());
            builder.ApplyConfiguration(new ObjectTypeConfig());
            builder.ApplyConfiguration(new ResponseParameterConfig());
            builder.ApplyConfiguration(new FieldConfig());
            // builder.ApplyConfiguration(new EntityParentConfig());
            builder.ApplyConfiguration(new EnumFieldConfig());
            builder.ApplyConfiguration(new FieldConfig());
        }
    }
}