using AutoMapper;
using BookIt.Application.Models.Reservation;
using BookIt.Application.Models.Table;
using BookIt.Application.Services;
using BookIt.Core.Entities.Identity;
using BookIt.DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace BookIt.Frontend.Pages.Reservation
{
    [Authorize(Roles = "Manager")]
    public class EditReservationModel : PageModel
    {
        private readonly IReservationService _reservationService;
        private readonly IRestaurantService _restaurantService;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IReservationRepository _reservationRepository;
        private readonly ITableRepository _tableRepository;
        private readonly ITableService _tableService;
        private readonly IMapper _mapper;
        public EditReservationModel(IRestaurantRepository restaurantRepository,
            ITableRepository tableRepository,
            IMapper mapper,
            IRestaurantService restaurantService,
            UserManager<ApplicationUser> userManager,
            IReservationRepository reservationRepository,
            IReservationService reservationService,
            ITableService tableService,
            IUserRepository userRepository)
        {
            _reservationService = reservationService;
            _mapper = mapper;
            _userManager = userManager;
            _tableRepository = tableRepository;
            _restaurantService = restaurantService;
            _tableService = tableService;
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
        }

        [FromRoute]
        public Guid Id { get; set; }


        [BindProperty]
        public ReservationModelForUpdate EditReservation { get; set; }

        public void OnGet()
        {
            var reservation = _reservationRepository.GetReservationByIdAsync(Id).Result;
            ViewData["ReservationDate"] = reservation.StartTime;
            ViewData["NumberOfPersons"] = reservation.NumberOfPersons;
            ViewData["RestaurantId"] = Id;
            EditReservation = _mapper.Map<ReservationModelForUpdate>(reservation);
        }

        public IActionResult OnPostEdit()
        {
            if (!ModelState.IsValid) { return Page(); }

            EditReservation.Id = Id.ToString();

            _reservationService.EditReservationAsync(EditReservation);

            return RedirectToPage("../Restaurant/ViewAllRestaurants");
        }
    }
}
