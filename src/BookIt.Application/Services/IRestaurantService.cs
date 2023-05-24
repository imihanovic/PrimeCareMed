using BookIt.Application.Models.Restaurant;
using BookIt.Core.Entities;

namespace BookIt.Application.Services
{
    public interface IRestaurantService
    {
        Task<RestaurantModel> AddAsync(RestaurantModelForCreate createRestaurantModel);

        IEnumerable<RestaurantModel> GetAllRestaurants();

        Restaurant EditRestaurantAsync(RestaurantModelForCreate restaurantModel);

        Task DeleteRestaurantAsync(Guid Id);

        List<string> GetRestaurantModelFields();

        IEnumerable<RestaurantModel> RestaurantSorting(IEnumerable<RestaurantModel> restaurants, string sortOrder);

        IEnumerable<RestaurantModel> RestaurantSearch(IEnumerable<RestaurantModel> restaurants, string searchString);
    }
}
