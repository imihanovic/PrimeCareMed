using BookIt.Application.Models.Restaurant;
using BookIt.Application.Models.User;
using BookIt.Application.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookIt.Frontend.Pages.Restaurant
{
    public class ViewAllRestaurantsModel : PageModel
    {
        public readonly IRestaurantService _restaurantService;

#nullable enable
        public PaginatedList<RestaurantModel> Restaurants { get; set; }

#nullable disable
        public List<string> RestaurantModelProperties;

        public int TotalPages { get; set; }

        public ViewAllRestaurantsModel(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        public void OnGet(string sort, string currentFilter, string keyword, int? pageIndex)
        {
            if (keyword != null)
            {
                pageIndex = 1;
            }
            else
            {
                keyword = currentFilter;
            }

            ViewData["CurrentFilter"] = keyword;
            int pageSize = 1;

            var restaurants = _restaurantService.GetAllRestaurants();

            RestaurantModelProperties = _restaurantService.GetRestaurantModelFields();


            ViewData["CurrentSort"] = sort;
            restaurants = _restaurantService.RestaurantSorting(restaurants, sort);

            ViewData["Keyword"] = keyword;
            restaurants = _restaurantService.RestaurantSearch(restaurants, keyword);

            Restaurants = PaginatedList<RestaurantModel>.Create(restaurants, pageIndex ?? 1, pageSize);

            TotalPages = (int)Math.Ceiling(decimal.Divide(restaurants.Count(), pageSize));
        }
    }
}
