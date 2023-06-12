using AutoMapper;
using BookIt.Application.Models.Reservation;
using BookIt.Application.Models.User;
using BookIt.Application.Services;
using BookIt.Core.Entities.Identity;
using BookIt.DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace BookIt.Frontend.Pages.Reservation
{
    [Authorize]
    public class ViewAllReservationModel : PageModel
    {
        public readonly IReservationService _reservationService;
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly IMapper _mapper;
        public readonly IRestaurantRepository _restaurantRepository;
        public readonly IReservationRepository _reservationRepository;

#nullable enable
        public PaginatedList<ReservationModel> Reservations { get; set; }

#nullable disable
        public List<string> ReservationModelProperties;

        public List<string> Slots { get; set; } = new List<string> { "12:00", "14:00", "16:00", "18:00", "20:00", "22:00" };

        public int TotalPages { get; set; }

        public ViewAllReservationModel(IReservationService reservationService,
            IRestaurantRepository restaurantRepository,
            IMapper mapper,
            IReservationRepository reservationRepository,
            UserManager<ApplicationUser> userManager)
        {
            _reservationRepository = reservationRepository;
            _reservationService = reservationService;
            _mapper = mapper;
            _restaurantRepository = restaurantRepository;
            _userManager = userManager;
        }

        public async Task OnGetAsync(string sort, string currentFilter, string keyword, string statusFilter, string reservationDate, string reservationTime, int? pageIndex)
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();


            if (keyword != null)
            {
                pageIndex = 1;
            }
            else
            {
                keyword = currentFilter;
            }

            ViewData["CurrentFilter"] = keyword;

            int pageSize = 2;

            ReservationModelProperties = _reservationService.GetReservationModelFields();

            var list = new List<ReservationModel>();

            IEnumerable<ReservationModel> reservations = list;


            if (currentUserRole == "Manager")
            {
                reservations = _reservationService.GetAllReservationsForManager(currentUser.Id);
            }
            else if (currentUserRole == "Customer")
            {
                reservations = _reservationService.GetAllReservationsForCustomer(currentUser.Id);
            }
            else
            {
                reservations = _reservationService.GetAllReservations();
            }

            await _reservationService.CheckReservationStatus(reservations);

            ViewData["CurrentSort"] = sort;
            reservations = _reservationService.ReservationSorting(reservations, sort);

            ViewData["StatusFilter"] = statusFilter;
            reservations = _reservationService.ReservationFilterByStatus(reservations, statusFilter);

            ViewData["ReservationDate"] = reservationDate;
            reservations = _reservationService.ReservationFilterByReservationDate(reservations, reservationDate);

            ViewData["ReservationTime"] = reservationTime;
            reservations = _reservationService.ReservationFilterByReservationTime(reservations, reservationTime);

            ViewData["Keyword"] = keyword;
            reservations = _reservationService.ReservationSearch(reservations, keyword);


            Reservations = PaginatedList<ReservationModel>.Create(reservations, pageIndex ?? 1, pageSize);

            TotalPages = (int)Math.Ceiling(decimal.Divide(reservations.Count(), pageSize));
        }
    }
}
