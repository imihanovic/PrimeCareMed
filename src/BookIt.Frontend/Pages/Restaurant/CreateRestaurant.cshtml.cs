using AutoMapper;
using BookIt.Application.Models.Restaurant;
using BookIt.Application.Models.User;
using BookIt.Application.Services;
using BookIt.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookIt.Frontend.Pages.Restaurant
{
    public class CreateRestaurantModel : PageModel
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRestaurantService _restaurantService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public CreateRestaurantModel(IRestaurantRepository restaurantRepository,
            IMapper mapper,
            IRestaurantService restaurantService,
            IUserService userService,
            IUserRepository userRepository)
        { 
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
            _userService = userService;
            _userRepository = userRepository;
            _restaurantService = restaurantService;
        }

        [BindProperty]
        public RestaurantModelForCreate NewRestaurant { get; set; }

        [BindProperty]
        public IEnumerable<ListUsersModel> Managers => _userService.GetAllManagers();


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { return Page(); }

            var restaurant = _mapper.Map<BookIt.Core.Entities.Restaurant>(NewRestaurant);
            try
            {
                await _restaurantService.AddAsync(NewRestaurant);
                return RedirectToPage("ViewAllRestaurants");
            }
            catch (Exception ex)
            {
                var manager = _userRepository.GetUserByIdAsync(NewRestaurant.ManagerId);
                ViewData["Message"] = string.Format($"Manager {manager.Result.UserName} is already selected for other restaurant!!!");
                Console.WriteLine(ex.Message);
            }
            return Page();
        }
    }
}
