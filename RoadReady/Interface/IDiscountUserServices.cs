using RoadReady.Models;

namespace RoadReady.Interface
{
    public interface IDiscountUserServices
    {
        Task<List<Discount>> ViewAvailableDiscounts();
        Task<Reservation> ApplyDiscountToReservation(int reservationId, string discountCode);
        Task<Discount> ViewDiscountDetails(int discountId);
    }
}
