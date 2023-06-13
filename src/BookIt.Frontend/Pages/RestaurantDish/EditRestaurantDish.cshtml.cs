using AutoMapper;
using BookIt.Application.Models.RestaurantDish;
using BookIt.Application.Models.Table;
using BookIt.Application.Services;
using BookIt.Core.Entities.Identity;
using BookIt.DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookIt.Frontend.Pages.RestaurantDish
{
    [Authorize(Roles = "Manager")]
    public class EditRestaurantDishModel : PageModel
    {
        private readonly IRestaurantDishRepository _restaurantDishRepository;
        private readonly IRestaurantDishService _restaurantDishService;
        private readonly IDishRepository _dishRepository;
        private readonly ITableService _tableService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public EditRestaurantDishModel(IRestaurantDishRepository restaurantDishRepository,
            IRestaurantDishService restaurantDishService,
            ITableService tableService,
            IDishRepository dishRepository,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _restaurantDishRepository = restaurantDishRepository;
            _restaurantDishService = restaurantDishService;
            _tableService = tableService;
            _dishRepository = dishRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        [FromRoute]
        public Guid Id { get; set; }

        [BindProperty]
        public RestaurantDishModelForUpdate EditRestaurantDish { get; set; }

        public IActionResult OnGet()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();

            var restaurantDish = _restaurantDishRepository.GetRestaurantDishByIdAsync(Id).Result;
            var restaurant = restaurantDish.Restaurant;
            var dish = restaurantDish.Dish;

            ViewData["RestaurantName"] = restaurant.RestaurantName;
            ViewData["Address"] = restaurant.Address;
            ViewData["RestaurantId"] = restaurant.Id;
            ViewData["DishId"] = dish.Id;
            ViewData["DishName"] = dish.DishName;
            ViewData["DishCategory"] = dish.Category;
            ViewData["DishPrice"] = restaurantDish.Price;
            ViewData["IsAvailible"] = restaurantDish.IsAvailable;


            EditRestaurantDish = _mapper.Map<RestaurantDishModelForUpdate>(restaurantDish);
            return Page();
        }
        public IActionResult OnPostEdit()
        {

            EditRestaurantDish.Id = Id.ToString();

            _restaurantDishService.EditRestaurantDish(EditRestaurantDish);

            return RedirectToPage("../Restaurant/ViewAllRestaurants");
        }
    }
}
