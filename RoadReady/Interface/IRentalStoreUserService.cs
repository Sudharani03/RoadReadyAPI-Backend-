using RoadReady.Models;
using System.Numerics;

namespace RoadReady.Interface
{
    public interface IRentalStoreUserService
    {
        public Task<List<RentalStore>> GetAllRentalStores();
        public Task<List<CarStore>> GetCarsInStore(int storeId);
    }
}
