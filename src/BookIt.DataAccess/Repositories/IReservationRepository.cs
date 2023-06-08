using BookIt.Core.Entities;

namespace BookIt.DataAccess.Repositories
{
    public interface IReservationRepository
    {
        Task<Reservation> AddAsync(Reservation reservation);

        Task<IEnumerable<Reservation>> GetAllReservationsAsync();

        Task<Reservation> GetReservationByIdAsync(Guid id);

        Task<Reservation> UpdateAsync(Reservation reservation);
    }
}
