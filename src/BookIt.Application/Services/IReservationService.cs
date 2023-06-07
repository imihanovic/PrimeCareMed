using BookIt.Application.Models.Reservation;
using BookIt.Core.Entities;

namespace BookIt.Application.Services
{
    public interface IReservationService
    {
        Task<Reservation> AddAsync(ReservationModelForCreate createReservationModel);
    }
}
