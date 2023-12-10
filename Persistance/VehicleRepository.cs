using Database.Entities;
using Microsoft.EntityFrameworkCore;
namespace Persistance
{
    public class VehicleRepository :IVehicleRepository
    {
        public readonly IQueryable<VehicleModel_Test> vehicles;
        public VehicleRepository(TransportDbContext dbcontext)
        {
            vehicles = dbcontext.Set<VehicleModel_Test>();
        }

        public IQueryable<VehicleModel_Test> GetAll()
        {
            return vehicles.AsNoTracking();
        }
    }
}
