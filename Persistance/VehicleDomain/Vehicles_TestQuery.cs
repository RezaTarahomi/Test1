using Application.VehicleDomain;
using Database.Entities;
using System.Linq;


namespace Persistance.VehicleDomain
{
    
    
    public class Vehicles_TestQuery : IVehicles_TestQuery
    {
        
        private IQueryable<Vehicles_Test> vehicles_tests;
        
        public Vehicles_TestQuery(TransportDbContext dbcontext)
        {
            vehicles_tests = dbcontext.Set<Vehicles_Test>();
        }
    }
}
