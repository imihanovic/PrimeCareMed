using BookIt.Application.Models.Reservation;
using BookIt.Core.Entities;

namespace BookIt.Application.Services
{
    public interface IReservationService
    {
        Task<Reservation> AddAsync(ReservationModelForCreate createReservationModel);

        IEnumerable<ReservationModel> GetAllReservations();

        Reservation EditReservationAsync(ReservationModelForUpdate reservationModel);

        List<string> GetReservationModelFields();

        IEnumerable<ReservationModel> ReservationSorting(IEnumerable<ReservationModel> reservations, string sortOrder);

        IEnumerable<ReservationModel> ReservationFilterByStatus(IEnumerable<ReservationModel> reservations, string status);

        IEnumerable<ReservationModel> ReservationSearch(IEnumerable<ReservationModel> reservations, string searchString);

        IEnumerable<ReservationModel> ReservationFilterByReservationDate(IEnumerable<ReservationModel> reservations, string reservationDate);

        IEnumerable<ReservationModel> ReservationFilterByReservationTime(IEnumerable<ReservationModel> reservations, string reservationTime);

        //IEnumerable<ReservationModel> GetAllReservationsByManagerIdAsync(IEnumerable<ReservationModel> reservations, string managerId);

        IEnumerable<ReservationModel> GetAllReservationsForCustomer(string customerId);

        IEnumerable<ReservationModel> GetAllReservationsForManager(string managerId);

    }
}
