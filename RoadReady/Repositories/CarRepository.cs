using Microsoft.EntityFrameworkCore;
using RoadReady.Contexts;
using RoadReady.Exceptions;
using RoadReady.Interface;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class CarRepository : IRepository<int,Car>
    {
        private readonly CarRentalDbContext _context;
        private readonly ILogger<CarRepository> _logger;
        public CarRepository(CarRentalDbContext context, ILogger<CarRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region --> AddCar
        public async Task<Car> Add(Car item)
        {
            // Checking if the CarID already exists
            var existingCar = await _context.Cars.FirstOrDefaultAsync(c => c.CarId == item.CarId);
            if (existingCar != null)
            {
                throw new CarAlreadyExistsException();
            }
            try
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Car added " + item.CarId);
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

        #region --> GetCarsList
        public async Task<List<Car>> GetAsync()
        {
            var cars = _context.Cars.Include(d => d.Discount).ToList();
            //statement checks if the list is empty or not.
            if (!cars.Any())
            {
                throw new CarListEmptyException();
            }
            return cars;
        }
        #endregion

        #region --> GetCarById
        public async Task<Car> GetAsyncById(int key)
        {
            var cars = await GetAsync();
            var car = cars.FirstOrDefault(c => c.CarId == key);
            if (car != null)
                return car;
            throw new NoSuchCarException();
        }
        #endregion

        #region --> UpdateCar
        public async Task<Car> Update(Car item)
        {
            var Car = await GetAsyncById(item.CarId);
            if (Car != null)
            {
                _context.Entry<Car>(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return item;
            }
            else
            {
                throw new NoSuchCarException();
            }
        }
        #endregion

        #region --> DeleteCar
        public async Task<Car> Delete(int key)
        {
            var car = await GetAsyncById(key);
            if (car != null)
            {
                _context?.Cars.Remove(car);
                await _context.SaveChangesAsync();
                return car;
            }
            else
            {
                throw new NoSuchCarException();
            }
        }
        #endregion
    }
}

