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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RequestParameterConfig());
            builder.ApplyConfiguration(new ResponseParameterConfig());
        }
    }
}
