using Application.Common;
using Application.VehicleDomain;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace Persistance.VehicleDomain
{
    
    
    public class Vehicles_TestRepository : IVehicles_TestRepository
    {
        
        private IQueryable<Vehicles_Test> vehicles_tests;
        private readonly TransportDbContext _dbcontext;
        
        public Vehicles_TestRepository(TransportDbContext dbcontext)
        {
            vehicles_tests = dbcontext.Set<Vehicles_Test>();
            _dbcontext = dbcontext;
        }

        public async Task<List<Id_Caption>> GetList()
        {
            return await vehicles_tests.AsNoTracking()
                .Select(x => new Id_Caption
                { 
                    Id=x.Id,
                    Caption=x.Capacity
                    
                }).ToListAsync();
        }

        public async Task AddAsync(Vehicles_Test entity)
        {
            await _dbcontext.Vehicles_Tests.AddAsync(entity);
            await _dbcontext.SaveChangesAsync();           
        }
        public async Task Save()
        {
            await _dbcontext.SaveChangesAsync();
        }

    }
}
