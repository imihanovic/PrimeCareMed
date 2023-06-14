using AutoMapper;
using BookIt.Application.Models.Reservation;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;
        public EditReservationModel(IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IReservationRepository reservationRepository,
            IReservationService reservationService
            )
        {
            _reservationService = reservationService;
            _mapper = mapper;
            _userManager = userManager;
            _reservationRepository = reservationRepository;
           
        }

        [FromRoute]
        public Guid Id { get; set; }


        [BindProperty]
        public ReservationModelForUpdate EditReservation { get; set; }

        public IActionResult OnGet()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            var reservation = _reservationRepository.GetReservationByIdAsync(Id).Result;
            var managerReservations = _reservationService.GetAllReservationsForManager(currentUser.Id).Select(r => r.Id);
            if (!managerReservations.Contains(reservation.Id))
            {
                return RedirectToPage("ViewAllReservation");
            }
            ViewData["ReservationDate"] = reservation.StartTime;
            ViewData["NumberOfPersons"] = reservation.NumberOfPersons;
            ViewData["RestaurantId"] = Id;
            EditReservation = _mapper.Map<ReservationModelForUpdate>(reservation);
            return Page();
        }

        public IActionResult OnPostEdit()
        {
            if (!ModelState.IsValid) { return Page(); }

            EditReservation.Id = Id.ToString();

            _reservationService.EditReservationAsync(EditReservation);

            return RedirectToPage("ViewAllReservation");
        }
    }
}
