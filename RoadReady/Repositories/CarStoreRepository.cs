using Microsoft.EntityFrameworkCore;
using RoadReady.Contexts;
using RoadReady.Exceptions;
using RoadReady.Interface;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class CarStoreRepository : IRepository<int, CarStore>
    {
        private readonly CarRentalDbContext _context;
        private readonly ILogger<CarStoreRepository> _logger;
        public CarStoreRepository(CarRentalDbContext context, ILogger<CarStoreRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        #region --> AddCarStore
        public async Task<CarStore> Add(CarStore item)
        {
            var existingCarStore = await _context.CarStore.FirstOrDefaultAsync(cs => cs.CarId == item.CarId);
            if (existingCarStore != null)
            {
                throw new CarAlreadyExistsException();
            }
            var existingCarStore1 = await _context.CarStore.FirstOrDefaultAsync(cs => cs.StoreId == item.StoreId);
            if (existingCarStore1 != null)
            {
                throw new CarStoreAlreadyExistsException();
            }
            try
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (DbUpdateException ex)
            {
                // Log the exception and return null
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                // Log the exception and return null
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }
        #endregion

        #region --> GetCarStoreList
        public async Task<List<CarStore>> GetAsync()
        {
            var carStores = _context.CarStore.Include(c => c.Car).Include(rs => rs.RentalStore).ToList();
            //statement checks if the list is empty or not.
            if (!carStores.Any())
            {
                throw new CarStoreListEmptyException();
            }
            return carStores;
        }
        #endregion

        #region --> GetCarStoreById
        public async Task<CarStore?> GetAsyncById(int key)
        {
            var carStores = await GetAsync();
            //FindAsync:retrieve an entity from the database by its primary key asynchronously.
            var carStore =await _context.CarStore.FindAsync(key); 
            if (carStore != null)
                return carStore;
            throw new NoSuchCarStoreException();
        }
        #endregion

        #region --> UpdateCarStore
        public async Task<CarStore> Update(CarStore item)
        {
            var CarStore = await GetAsyncById(item.CarId);
            if (CarStore != null)
            {
                _context.Entry<CarStore>(item).State = EntityState.Modified;
                //the above statement generates a update query when the save changes is called for,
                //for all attributes except the primary  key. 
                //Use the primary key in the where clause of the update query
                await _context.SaveChangesAsync();
                return item;
            }
            else
            {
                throw new NoSuchCarStoreException();
            }
        }
        #endregion

        #region --> DeleteCarStore
        public async Task<CarStore> Delete(int key)
        {
            var carStore = await GetAsyncById(key);
            if (carStore != null)
            {
                _context?.CarStore.Remove(carStore);
                await _context.SaveChangesAsync();
                return carStore;
            }
            else
            {
                throw new NoSuchCarStoreException();
            }
        }
        #endregion
    }
}
