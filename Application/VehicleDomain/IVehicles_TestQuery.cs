using Database.Entities;

namespace Application.VehicleDomain
{
    public interface IVehicles_TestQuery
    {
        Task<Vehicles_Test?> FindById(int id);
        Task<List<Vehicles_Test>> GetAll();
    }
}