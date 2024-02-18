using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoadReady.Exceptions;
using RoadReady.Interface;
using RoadReady.Models;
using RoadReady.Repositories;
using System.Linq;
using System.Numerics;

namespace RoadReady.Services
{
    public class CarService : ICarAdminService,ICarUserService
    {
        private readonly IRepository<int, Car> _carRepository;
        private readonly IRepository<int, Discount> _discountRepository;
        private readonly ILogger<CarService> _logger;
        public CarService(IRepository<int, Car> carRepository, IRepository<int, Discount> discountRepository, ILogger<CarService> logger)
        {
            _discountRepository = discountRepository;
            _carRepository = carRepository;
            _logger = logger;
        }
        
        #region ---> AddCar

        /// <summary>
        /// Adds a Car to the list
        /// </summary>
        /// <param name="car"></param>
        /// <returns>returns the car</returns>
        /// 
        public async Task<Car> AddCar(Car car)
        {
            try
            {
                return await _carRepository.Add(car);
            }
            catch (CarAlreadyExistsException ex)
            {
                _logger.LogWarning($"Car with ID {car.CarId} already exists.{ex.Message}");
                throw; // Re-throw the exception
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while adding the car: {ex.Message}");
                throw; // Re-throw the exception
            }
        }

        #endregion


        #region --->AddDiscountToCar

        /// <summary>
        /// Adds Discount to Car
        /// </summary>
        /// <param name="carId"></param>
        /// <param name="discount"></param>
        /// <returns>Updates and returns the car </returns>
        /// <exception cref="DiscountAlreadyExistsException"></exception>
        /// <exception cref="NoSuchCarException"></exception>
        public async Task<Car> AddDiscountToCar(int carId, int discountId)
        {
            try
            {
                var existingDiscount = await _discountRepository.GetAsyncById(discountId);
                var car = await _carRepository.GetAsyncById(carId);

                if (existingDiscount != null && car != null)
                {
                    car.Discounts.Add(existingDiscount);
                    car = await _carRepository.Update(car);
                    // Save changes to the database
                    return car;
                }
                else
                {
                    // Handle case where discount or car is not found
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                return null;
            }
        }

        #endregion


        #region ---> DeleteCar

        /// <summary>
        /// Deletes the Car based on id from list
        /// </summary>
        /// <param name="id"></param>
        /// <returns>returns the deleted car</returns>
        /// 
        public async Task<Car> DeleteCar(int id)
        {
            try
            {
                return await _carRepository.Delete(id);
            }
            catch (NoSuchCarException ex)
            {
                _logger.LogWarning($"Car with ID {id} not found.");
                throw; // Re-throw the exception
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting the car: {ex.Message}");
                throw; // Re-throw the exception
            }
        }

        #endregion


        #region ---> GetCarById

        /// <summary>
        /// Gets the car by Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Car by Id </returns>
        public async Task<Car> GetCarById(int id)
        {
            try
            {
                return await _carRepository.GetAsyncById(id);
            }
            catch (NoSuchCarException ex)
            {
                _logger.LogWarning($"Car with ID {id} not found.");
                throw; // Re-throw the exception
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting the car by ID: {ex.Message}");
                throw; // Re-throw the exception
            }

        }

        #endregion


        #region ---> GetCarsByAvailabiltyStatus

        /// <summary>
        /// Gets Cars By Availabilty Status
        /// </summary>
        /// <returns>Cars</returns>
        public async Task<List<Car>> GetCarsByAvailabiltyStatus()
        {
            List<Car> allCars = await _carRepository.GetAsync();
            List<Car> availableCars =allCars.Where(c => c.Availability == true).ToList();
            return availableCars;
        }

        #endregion


        #region ---> GetCarsList

        /// <summary>
        /// Gets the cars list
        /// </summary>
        /// <returns>cars</returns>
        /// <exception cref="CarListEmptyException"></exception>
        public async Task<List<Car>> GetCarsList()
        {
            try
            {
                var cars = await _carRepository.GetAsync();
                if (cars == null || cars.Count == 0)
                {
                    throw new CarListEmptyException();
                }
                return cars;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting the list of cars: {ex.Message}");
                throw; // Re-throw the exception
            }
        }

        #endregion


        #region ---> MakeReservation

        /// <summary>
        /// Makes the Reservation
        /// </summary>
        /// <param name="carId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>Reservation</returns>
        /// <exception cref="NoSuchCarException"></exception>
        /// <exception cref="InvalidReservationDatesException"></exception>
        /// <exception cref="ReservationConflictException"></exception>
        public async Task<Reservation> MakeReservation(int carId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var car = await _carRepository.GetAsyncById(carId) ?? throw new NoSuchCarException();

                // Check car availability
                if (car.Availability != true)
                {
                    _logger.LogWarning($"Car with ID {carId} is not available for reservation");
                    throw new NoSuchCarException();
                }

                // Check if the requested dates are valid
                if (startDate >= endDate)
                {
                    _logger.LogWarning($"Invalid reservation dates. Start date must be before end date.");
                    throw new InvalidReservationDatesException();
                }

                // Check if there are overlapping reservations
                if (car.Reservations.Any(reservation =>
                    (startDate >= reservation.PickUpDateTime && startDate <= reservation.DropOffDateTime) ||
                    (endDate >= reservation.PickUpDateTime && endDate <= reservation.DropOffDateTime)))
                {
                    _logger.LogWarning($"There is already a reservation for the specified dates on car with ID {carId}");
                    throw new ReservationConflictException();
                }

                // Create a new reservation
                var reservation = new Reservation
                {
                    CarId = carId,
                    PickUpDateTime = startDate,
                    DropOffDateTime = endDate
                    // Add other reservation properties as needed
                };

                // Add the reservation to the car
                car.Reservations.Add(reservation);

                // Update the car in the repository
                await _carRepository.Update(car);

                return reservation;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in MakeReservation: {ex.Message}");
                throw;
            }
        }

        #endregion


        #region ---> RemoveDiscountFromCar
        
        /// <summary>
        /// Removes the discount from car
        /// </summary>
        /// <param name="carId"></param>
        /// <param name="discountId"></param>
        /// <returns>Car</returns>
        /// <exception cref="NoSuchCarException"></exception>
        /// <exception cref="DiscountNotAssignedToCarException"></exception>

        public async Task<Car> RemoveDiscountFromCar(int carId, int discountId)
        {
            try
            {
                var car = await _carRepository.GetAsyncById(carId) ?? throw new NoSuchCarException();

                // Check if the car has the specified discount
                var discountToRemove = car.Discounts.FirstOrDefault(d => d.DiscountId == discountId);
                if (discountToRemove != null)
                {
                    // Remove the discount from the car
                    car.Discounts.Remove(discountToRemove);

                    // Update the car in the repository
                    await _carRepository.Update(car);

                    return car;
                }
                else
                {
                    _logger.LogWarning($"The car with ID {carId} does not have the specified discount with ID {discountId}");
                    throw new DiscountNotAssignedToCarException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in RemoveDiscountFromCar: {ex.Message}");
                throw;
            }
        }

        #endregion


        #region ---> UpdateCarDailyRate

        /// <summary>
        /// Updates Cars daily rate 
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="DailyRate"></param>
        /// <returns>car</returns>
        public async Task<Car> UpdateCarDailyRate(int carid, double DailyRate)
        {
            try
            {
                var car = await _carRepository.GetAsyncById(carid);
                if (car != null)
                {
                    car.DailyRate = DailyRate;
                    return await _carRepository.Update(car);
                }
                return null;
            }
            catch (NoSuchCarException ex)
            {
                _logger.LogError(ex.Message);
                throw;// Re-throw the exception
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating the car details: {ex.Message}");
                throw; // Re-throw the exception
            }
        }

        #endregion


        #region ---> UpdateCarAvailibility

        /// <summary>
        /// UpdateCarAvailibility
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="Availability"></param>
        /// <returns>Car</returns>

        public async Task<Car> UpdateCarAvailibility(int carid, bool Availability)
        {
            try
            {
                var car = await _carRepository.GetAsyncById(carid);
                if (car != null)
                {
                    car.Availability = Availability;
                    return await _carRepository.Update(car);
                }
                return null;
            }
            catch (NoSuchCarException ex)
            {
                _logger.LogError(ex.Message);
                throw; // Re-throw the exception
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating the car details: {ex.Message}");
                throw; // Re-throw the exception
            }
        }

        #endregion


        #region ---> UpdateCarSpecification
        /// <summary>
        /// Updates Car Specification
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="Specification"></param>
        /// <returns>Car</returns>
        public async Task<Car> UpdateCarSpecification(int carid, string Specification)
        {
            try
            {
                var car = await _carRepository.GetAsyncById(carid);
                if (car != null)
                {
                    car.Specification = Specification;
                    return await _carRepository.Update(car);
                }
                return null;
            }
            catch (NoSuchCarException ex)
            {
                _logger.LogError(ex.Message);
                throw; // Re-throw the exception
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating the car details: {ex.Message}");
                throw; // Re-throw the exception
            }
        }

        #endregion


        #region ---> ViewAllReservations

        /// <summary>
        /// View all reservations
        /// </summary>
        /// <returns>reservation</returns>

        public async Task<List<Reservation>> ViewAllReservations()
        {
            try
            {
                var allCars = await _carRepository.GetAsync();
                var allReservations = allCars.SelectMany(car => car.Reservations).ToList();
                return allReservations;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ViewAllReservations: {ex.Message}");
                throw;
            }
        }

        #endregion


        #region ---> ViewAvailableCars

        /// <summary>
        /// View Available Cars
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>Cars</returns>

        public async Task<List<Car>> ViewAvailableCars(DateTime startDate, DateTime endDate)
        {
            try
            {
                var allCars = await _carRepository.GetAsync();

                // Filter available cars based on reservations and dates

                var availableCars = allCars
                    .Where(car => car.Availability == true && // Check if the car is available
                        (car.Reservations == null || // Check if reservations collection is null
                             !car.Reservations.Any(reservation => // Check if any reservation overlaps
                          reservation.DropOffDateTime <= endDate && reservation.PickUpDateTime >= startDate))).ToList();

                return availableCars;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ViewAvailableCars: {ex.Message}");
                throw;
            }
        }

        #endregion


        #region --->ViewCarAvailability

        /// <summary>
        /// View Car Availability
        /// </summary>
        /// <param name="carId"></param>
        /// <returns>Available or not </returns>
        /// <exception cref="NoSuchCarException"></exception>
        public async Task<string> ViewCarAvailability(int carId)
        {
            try
            {
                var car = await _carRepository.GetAsyncById(carId) ?? throw new NoSuchCarException();

                // Check if the car is available
                if (car.Availability ?? false)
                {
                    return "Car is Available";
                }
                else
                {
                    return "Car is Not Available";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ViewCarAvailability: {ex.Message}");
                throw;
            }
        }

        #endregion


        #region ---> ViewCarDetails

        /// <summary>
        /// View Car details 
        /// </summary>
        /// <param name="carId"></param>
        /// <returns>Car</returns>
        /// <exception cref="NoSuchCarException"></exception>
        public async Task<Car> ViewCarDetails(int carId)
        {
            try
            {
                var car = await _carRepository.GetAsyncById(carId) ?? throw new NoSuchCarException();

                // Return the details of the car
                return car;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ViewCarDetails: {ex.Message}");
                throw;
            }
        }

        #endregion


        #region ---> ViewPastReservations 

        /// <summary>
        /// View Past Reservations 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>pastReservations</returns>

        public async Task<List<Reservation>> ViewPastReservations(int userId)
        {
            try
            {
                var allCars = await _carRepository.GetAsync();
                var pastReservations = allCars.SelectMany(car => car.Reservations)
                                              .Where(reservation => reservation.UserId == userId && reservation.DropOffDateTime < DateTime.Now)
                                              .ToList();

                return pastReservations;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ViewPastReservations: {ex.Message}");
                throw;
            }
        }

        #endregion


        #region ---> ViewPayments

        /// <summary>
        /// Views payments
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Payments</returns>
        public async Task<List<Payment>> ViewPayments(int userId)
        {
            try
            {
                var allCars = await _carRepository.GetAsync();
                var payments = allCars.SelectMany(car => car.Payments)
                                      .Where(payment => payment.UserId == userId)
                                      .ToList();

                return payments;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ViewPayments: {ex.Message}");
                throw;
            }
        }

        #endregion


        #region ---> ViewReservationDetails

        /// <summary>
        /// View Reservation Details 
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns>reservation</returns>
        /// <exception cref="NoSuchReservationException"></exception>
        public async Task<Reservation> ViewReservationDetails(int reservationId)
        {
            try
            {
                var allCars = await _carRepository.GetAsync();
                var reservation = allCars.SelectMany(car => car.Reservations)
                                        .FirstOrDefault(res => res.ReservationId == reservationId);

                if (reservation != null)
                {
                    return reservation;
                }
                else
                {
                    _logger.LogWarning($"Reservation with ID {reservationId} not found");
                    throw new NoSuchReservationException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ViewReservationDetails: {ex.Message}");
                throw;
            }
        }

        #endregion


        #region ---> ViewReservationDetailsForAdmin

        /// <summary>
        /// Reservation details for admin
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        /// <exception cref="NoSuchReservationException"></exception>

        public async Task<Reservation> ViewReservationDetailsForAdmin(int reservationId)
        {
            try
            {
                var allCars = await _carRepository.GetAsync();
                var reservation = allCars.SelectMany(car => car.Reservations)
                                        .FirstOrDefault(res => res.ReservationId == reservationId);

                if (reservation != null)
                {
                    return reservation;
                }
                else
                {
                    _logger.LogWarning($"Reservation with ID {reservationId} not found");
                    throw new NoSuchReservationException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ViewReservationDetailsForAdmin: {ex.Message}");
                throw;
            }
        }

        #endregion


        #region ---> ViewReviews

        /// <summary>
        /// View reviews 
        /// </summary>
        /// <param name="carId"></param>
        /// <returns>reviews list</returns>
        /// <exception cref="NoSuchCarException"></exception>

        public async Task<List<Review>> ViewReviews(int carId)
        {
            try
            {
                var car = await _carRepository.GetAsyncById(carId) ?? throw new NoSuchCarException();

                // Return the reviews for the specified car
                return car.Reviews.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ViewReviews: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Car>> GetCarsByAvailabilityStatus()
        {
            List<Car> allCars = await _carRepository.GetAsync();
            List<Car> availableCars = allCars.Where(c => c.Availability == true).ToList();
            return availableCars;
        }

        #endregion
    }
}
