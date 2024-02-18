using RoadReady.Models;

namespace RoadReady.Interface
{
    public interface ICarAdminService
    {
        public Task<List<Car>> GetCarsList();
        public Task<Car> AddCar(Car car);
        public Task<Car> UpdateCarSpecification(int carid, string Specification);
        public Task<Car> UpdateCarAvailibility(int carid, bool Availability);
        public Task<Car> UpdateCarDailyRate(int carid, double DailyRate);
        public Task<Car> DeleteCar(int id);
        public Task<Car> AddDiscountToCar(int carId, int discountId);
        public Task<Car> RemoveDiscountFromCar(int carId, int discountId);
        public Task<string> ViewCarAvailability(int carId);
        public Task<List<Reservation>> ViewAllReservations();
        public Task<Reservation> ViewReservationDetailsForAdmin(int reservationId);
        public Task<List<Car>> GetCarsByAvailabiltyStatus();
    }
}
