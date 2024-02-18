using RoadReady.Models;

namespace RoadReady.Interface
{
    public interface IReservationUserService
    {
        public Task<Reservation> GetReservationDetails(int id);
        public Task<Reservation> GetReservsationStatus(int id);
        public Task<Reservation> CreateReservation(Reservation reservation);
        public Task<List<Reservation>> GetUserReservations(int userId);
        public Task<bool> CancelReservation(int reservationId);

    }
}
