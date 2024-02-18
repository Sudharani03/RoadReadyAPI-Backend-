using RoadReady.Models;

namespace RoadReady.Interface
{
    public interface ICarStoreUserServices
    {
        Task<List<Car>> ViewCarsInStore(int storeId);
    }
}
