using AutoMapper;
using BookIt.Application.Models.Restaurant;
using BookIt.Application.Models.User;
using BookIt.Application.Models.Dish;
using BookIt.Application.Services;
using BookIt.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookIt.Application.Models.RestaurantDish;
using Microsoft.AspNetCore.Identity;
using BookIt.Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;

namespace BookIt.Frontend.Pages.RestaurantDish
{
    [Authorize(Roles = "Manager")]
    public class CreateRestaurantDishModel : PageModel
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRestaurantDishService _restaurantDishService;
        private readonly IRestaurantService _restaurantService;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDishRepository _dishRepository;
        private readonly IDishService _dishService;
        private readonly IMapper _mapper;
        public CreateRestaurantDishModel(IRestaurantRepository restaurantRepository,
            IMapper mapper,
            IRestaurantService restaurantService,
            IDishService dishService,
            IRestaurantDishService restaurantDishService,
            UserManager<ApplicationUser> userManager,
            IDishRepository dishRepository,
            IUserService userService,
            IUserRepository userRepository)
        {
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
            _dishRepository = dishRepository;
            _restaurantDishService = restaurantDishService;
            _dishService = dishService;
            _userService = userService;
            _userRepository = userRepository;
            _userManager = userManager;
            _restaurantService = restaurantService;
        }

        [BindProperty]
        public RestaurantDishModelForCreate NewRestaurantDish { get; set; }

        [BindProperty]
        public IEnumerable<ListUsersModel> Managers => _userService.GetAllManagers();

        [BindProperty]
        public IEnumerable<DishModel> Dishes => _dishService.GetAllDishes();

        [FromRoute]
        public Guid DishId { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();

            if (!ModelState.IsValid) { return Page(); }

            var restaurant = currentUser.Restaurant;

            if(restaurant is not  null)
            {
                ViewData["RestaurantName"] = restaurant.RestaurantName;
                ViewData["RestaurantOwner"] = restaurant.RestaurantOwner;
                ViewData["Address"] = restaurant.Address;
            }

            var dish = _dishService.GetAllDishes().FirstOrDefault(d => d.Id == DishId);
            if(dish is not null)
            {
                ViewData["DishName"] = dish.DishName;
                ViewData["DishCategory"] = dish.Category;
                ViewData["DishDescription"] = dish.DishDescription;
            }
            
            try
            {
                await _restaurantDishService.AddAsync(NewRestaurantDish);
                return RedirectToPage("ViewAllRestaurantDishes");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Page();
        }
    }
}
