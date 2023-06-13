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

        IEnumerable<RestaurantDishModel> RestaurantDishFilter(IEnumerable<RestaurantDishModel> restaurantDishes, string category);

        List<string> GetRestaurantModelFields();

        Task DeleteRestaurantDishAsync(Guid Id);
    }
}
