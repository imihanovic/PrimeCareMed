using BookIt.Application.Models.User;
using BookIt.Application.Models.RestaurantDish;
using BookIt.Application.Services;
using BookIt.Core.Entities.Identity;
using BookIt.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace BookIt.Frontend.Pages.RestaurantDish
{
    [Authorize]
    public class ViewAllRestaurantDishesModel : PageModel
    {
        public readonly IDishService _dishService;
        public readonly IDishRepository _dishRepository;
        private readonly IRestaurantDishRepository _restaurantDishRepository;
        public readonly IRestaurantDishService _restaurantDishService;
        public readonly IRestaurantRepository _restaurantRepository;
        public readonly UserManager<ApplicationUser> _userManager;
#nullable enable
        public PaginatedList<RestaurantDishModel> RestaurantDishes { get; set; }

#nullable disable
        public List<string> RestaurantDishModelProperties;

        [FromRoute]
        public Guid Id { get; set; }

        public int TotalPages { get; set; }

        public ViewAllRestaurantDishesModel(IDishService dishService,
            IDishRepository dishRepository,
            IRestaurantDishRepository restaurantDishRepository,
            IRestaurantDishService restaurantDishService,
            IRestaurantRepository restaurantRepository,
            UserManager<ApplicationUser> userManager)
        {
            _dishService = dishService;
            _userManager = userManager;
            _restaurantDishRepository = restaurantDishRepository;
            _restaurantRepository = restaurantRepository;
            _dishRepository = dishRepository;
            _restaurantDishService = restaurantDishService;
        }

        public void OnGet(string currentFilter, string keyword, string categoryFilter, int? pageIndex)
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
            var restaurant = _restaurantRepository.GetRestaurantByIdAsync(Id).Result;

            ViewData["RestaurantName"] = restaurant.RestaurantName;
            ViewData["Address"] = restaurant.Address;

            RestaurantDishModelProperties = _restaurantDishService.GetRestaurantModelFields(currentUserRole);

            ViewData["CurrentFilter"] = keyword;
            int pageSize = 4;

            var restaurantDishes = _restaurantDishService.GetAllRestaurantDishesByRestaurantId(Id.ToString());

            if (currentUserRole == "Customer")
            {
                restaurantDishes = restaurantDishes.Where(r => r.IsAvailable == true);
            }

            ViewData["Keyword"] = keyword;
            restaurantDishes = _restaurantDishService.RestaurantDishSearch(restaurantDishes, keyword);

            ViewData["CategoryFilter"] = categoryFilter;
            restaurantDishes = _restaurantDishService.RestaurantDishFilter(restaurantDishes, categoryFilter);

            RestaurantDishes = PaginatedList<RestaurantDishModel>.Create(restaurantDishes, pageIndex ?? 1, pageSize);

            TotalPages = (int)Math.Ceiling(decimal.Divide(restaurantDishes.Count(), pageSize));

        }
        public IActionResult OnPostDelete(Guid id)
        {
            var restaurantDish = _restaurantDishRepository.GetRestaurantDishByIdAsync(Id).Result;
            var restaurant = restaurantDish.Restaurant;

            _restaurantDishService.DeleteRestaurantDishAsync(id);
            return RedirectToPage("ViewAllRestaurantDishes", new { id = restaurant.Id.ToString() });
        }
    }
}
