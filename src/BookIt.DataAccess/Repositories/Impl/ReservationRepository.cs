using BookIt.Core.Entities;
using BookIt.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace BookIt.DataAccess.Repositories.Impl
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly DatabaseContext _context;
        public ReservationRepository(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Reservation> AddAsync(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            return await _context.Reservations.OrderBy(r => r.Id).Include(r => r.Customer).Include(r => r.Tables).ThenInclude(r => r.Restaurant).ToListAsync(); 
        }

        public async Task<Reservation> GetReservationByIdAsync(Guid id)
        {
            return await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Reservation> UpdateAsync(Reservation reservation)
        {
            var editItem = await GetReservationByIdAsync(reservation.Id);
            editItem.Status = reservation.Status;
            Console.WriteLine($"Reservation details after post: {reservation.ReservationDetails}");
            editItem.ReservationDetails = reservation.ReservationDetails;
            await _context.SaveChangesAsync();
            return editItem;
        }
    }
}
