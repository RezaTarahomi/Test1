//using Application.VehicleDomain;
//using Database.Entities;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;

//namespace Persistance.VehicleDomain
//{
//    public class Vehicles_TestQuery : IVehicles_TestQuery
//    {
//        private IQueryable<Vehicles_Test> vehicles_tests;
//        public Vehicles_TestQuery(TransportDbContext dbcontext)
//        {
//            vehicles_tests = dbcontext.Set<Vehicles_Test>();
//        }

//        public async Task<Vehicles_Test?> FindById(int id)
//        {
//            return await vehicles_tests.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
//        }

//        public async Task<List<Vehicles_Test>> GetAll()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}