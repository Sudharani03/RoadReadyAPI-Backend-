using RoadReady.Models;
using System.Numerics;

namespace RoadReady.Interface
{
    public interface IRentalStoreAdminService
    {
        public Task<List<RentalStore>> GetAllRentalStores();
        public Task<RentalStore> AddRentalStore(RentalStore rentalStore);
        public Task<RentalStore> UpdateRentalStoreDetails(int storeId, string pickUpLocation, string dropOffLocation);
        public Task<RentalStore> RemoveRentalStore(int id);
        public Task<RentalStore> GetRentalStoreById(int storeId);
        public Task<List<CarStore>> GetCarsInStore(int storeId);
    }
}
