

using Database.Entities;

namespace Persistance
{
    public interface IVehicleRepository
    {
        IQueryable<VehicleModel_Test> GetAll();
    }
}
