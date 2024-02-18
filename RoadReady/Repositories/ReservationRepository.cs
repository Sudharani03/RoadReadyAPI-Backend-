using Microsoft.EntityFrameworkCore;
using RoadReady.Contexts;
using RoadReady.Exceptions;
using RoadReady.Interface;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class ReservationRepository : IRepository<int, Reservation>
    {
        private readonly CarRentalDbContext _context;
        private readonly ILogger<ReservationRepository> _logger;
        public ReservationRepository(CarRentalDbContext context, ILogger<ReservationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region ---> AddReservation 
        public async Task<Reservation> Add(Reservation item)
        {
            // Check if the ReservationID already exists
            var existingReservation = await _context.Reservations.FirstOrDefaultAsync(res => res.ReservationId == item.ReservationId);
            if (existingReservation != null)
            {
                throw new ReservationAlreadyExistsException();
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


        #region ---> GetReservationList
        public async Task<List<Reservation>> GetAsync()
        {
            var reservations = _context.Reservations.Include(p => p.Payment).Include(u => u.User).Include(c => c.Car).ToList();
            //statement checks if the list is empty or not.
            if (!reservations.Any())
            {
                throw new ReservationListEmptyException();
            }
            return reservations;
        }

        #endregion


        #region ---> GetReservationById

        public async Task<Reservation> GetAsyncById(int key)
        {
            var reservations = await GetAsync();
            var reservation = reservations.FirstOrDefault(res => res.ReservationId == key);
            if (reservation != null)
                return reservation;
            throw new NoSuchReservationException();
        }
        #endregion


        #region ---> UpdateReservation
        public async Task<Reservation> Update(Reservation item)
        {
            var Reservation = await GetAsyncById(item.ReservationId);
            if (Reservation != null)
            {
                _context.Entry<Reservation>(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return item;
            }
            else
            {
                throw new NoSuchReservationException();
            }
        }

        #endregion


        #region ---> DeleteReservation
        public async Task<Reservation> Delete(int key)
        {
            var reservation = await GetAsyncById(key);
            if (reservation != null)
            {
                _context?.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
                return reservation;
            }
            else
            {
                throw new NoSuchReservationException();
            }
        }

        #endregion
    }

}
