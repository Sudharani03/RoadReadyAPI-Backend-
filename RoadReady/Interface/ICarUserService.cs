using RoadReady.Models;

namespace RoadReady.Interface
{
    public interface ICarUserService
    {
        Task<Car> ViewCarDetails(int carId);

        Task<List<Car>> ViewAvailableCars(DateTime startDate, DateTime endDate);

        Task<Reservation> MakeReservation(int carId, DateTime startDate, DateTime endDate);

        Task<Reservation> ViewReservationDetails(int reservationId);

        Task<List<Reservation>> ViewPastReservations(int userId);

        Task<Car> GetCarById(int id);
        Task<List<Car>> GetCarsByAvailabilityStatus();
    }
}
