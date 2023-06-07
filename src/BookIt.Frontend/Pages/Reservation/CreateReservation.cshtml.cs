using AutoMapper;
using BookIt.Application.Models.Reservation;
using BookIt.Application.Models.Restaurant;
using BookIt.Application.Models.Table;
using BookIt.Application.Models.User;
using BookIt.Application.Services;
using BookIt.Application.Services.Impl;
using BookIt.Core.Entities;
using BookIt.DataAccess.Repositories;
using BookIt.DataAccess.Repositories.Impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookIt.Frontend.Pages.Reservation
{
    public class CreateReservationModel : PageModel
    {
        private readonly IReservationService _reservationService;
        private readonly IRestaurantService _restaurantService;
        private readonly IUserRepository _userRepository;
        private readonly ITableService _tableService;
        private readonly IMapper _mapper;
        public CreateReservationModel(IRestaurantRepository restaurantRepository,
            IMapper mapper,
            IRestaurantService restaurantService,
            IReservationService reservationService,
            ITableService tableService,
            IUserRepository userRepository)
        {
            _reservationService = reservationService;
            _mapper = mapper;
            _restaurantService = restaurantService;
            _tableService = tableService;
            _userRepository = userRepository;
        }

        [BindProperty]
        public ReservationModelForCreate NewReservation { get; set; }

        [BindProperty]
        public IEnumerable<TableModel> Tables { get; set; }

        public void OnGet(int numberOfPersons, string tableArea, string smokingArea, string reservationDate, Guid restaurantId)
        {
            var restaurant = _restaurantService.GetAllRestaurants().FirstOrDefault(r => r.Id == restaurantId);
            if (restaurant != null)
            {
                ViewData["RestaurantName"] = restaurant.RestaurantName;
                ViewData["Address"] = restaurant.Address;
            }
            ViewData["ReservationDate"] = reservationDate;
            ViewData["NumberOfPersons"] = numberOfPersons;

            var tables = _tableService.GetAllTables(restaurantId);

            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _reservationService.AddAsync(NewReservation);
                return RedirectToPage("../Restaurant/ViewAllRestaurants");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Page();
        }
    }
}
