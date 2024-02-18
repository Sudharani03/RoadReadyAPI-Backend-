using RoadReady.Models;
using System.Threading.Tasks;

namespace RoadReady.Interface
{
    public interface IReservationAdminService : IReservationUserService
    {
        Task<List<Reservation>> GetAllReservations();
        Task<List<Reservation>> GetPendingReservations();
        Task<Reservation> UpdateReservationStatus(int reservationId, string newStatus);
        Task<Reservation> UpdateReservationPrice(int reservationId, double newPrice);

    }
}
