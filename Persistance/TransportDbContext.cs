using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistance
{
    public class TransportDbContext :DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<Vehicles_Test> Vehicles_Tests { get; set; }
        
    }
}
