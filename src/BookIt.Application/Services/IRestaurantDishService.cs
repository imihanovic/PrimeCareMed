using BookIt.Application.Models.RestaurantDish;
using BookIt.Core.Entities;

namespace BookIt.Application.Services
{
    public interface IRestaurantDishService
    {
        Task<RestaurantDishModel> AddAsync(RestaurantDishModelForCreate createRestaurantDishModel);

        IEnumerable<RestaurantDishModel> GetAllRestaurantDishesByRestaurantId(string restaurantId);

        RestaurantDish EditRestaurantDish(RestaurantDishModelForUpdate restaurantDishModel);

        IEnumerable<RestaurantDishModel> RestaurantSorting(IEnumerable<RestaurantDishModel> restaurantDishes, string sortOrder);

        IEnumerable<RestaurantDishModel> RestaurantDishSearch(IEnumerable<RestaurantDishModel> restaurantDishes, string searchString);

        List<string> GetRestaurantModelFields();
    }
}
