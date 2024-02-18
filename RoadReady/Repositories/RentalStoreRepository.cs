using Microsoft.EntityFrameworkCore;
using RoadReady.Contexts;
using RoadReady.Exceptions;
using RoadReady.Interface;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class RentalStoreRepository : IRepository<int, RentalStore>
    {
        private readonly CarRentalDbContext _context;
        private readonly ILogger<RentalStoreRepository> _logger;
        public RentalStoreRepository(CarRentalDbContext context, ILogger<RentalStoreRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region --> AddRentalStore
        public async Task<RentalStore> Add(RentalStore item)
        {
            // Check if the StoreID already exists
            var existingRentalStore = await _context.RentalStores.FirstOrDefaultAsync(s => s.StoreId == item.StoreId);
            if (existingRentalStore != null)
            {
                throw new RentalStoreAlreadyExistsException();
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

        #region --> GetRentalStoresList
        public async Task<List<RentalStore>> GetAsync()
        {
            var rentalStores = await _context.RentalStores.ToListAsync();
            //statement checks if the list is empty or not.
            if (!rentalStores.Any())
            {
                throw new RentalStoreListEmptyException();
            }
            return rentalStores;
        }
        #endregion

        #region --> GetRentalStoreById
        public async Task<RentalStore> GetAsyncById(int key)
        {
            var rentalStores = await GetAsync();
            var rentalStore = rentalStores.FirstOrDefault(r => r.StoreId == key);
            if (rentalStore != null)
                return rentalStore;
            throw new NoSuchRentalStoreException();
        }
        #endregion

        #region --> UpdateRentalStore
        public async Task<RentalStore> Update(RentalStore item)
        {
            var RentalStore = await GetAsyncById(item.StoreId);
            if (RentalStore != null)
            {
                _context.Entry<RentalStore>(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return item;
            }
            else
            {
                throw new NoSuchRentalStoreException();
            }
        }
        #endregion

        #region --> DeleteRentalStore
        public async Task<RentalStore> Delete(int key)
        {
            var rentalStore = await GetAsyncById(key);
            if (rentalStore != null)
            {
                _context?.RentalStores.Remove(rentalStore);
                await _context.SaveChangesAsync();
                return rentalStore;
            }
            else
            {
                throw new NoSuchRentalStoreException();
            }
        }
        #endregion
    }
}

