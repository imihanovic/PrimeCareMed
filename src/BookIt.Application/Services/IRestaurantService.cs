using BookIt.Application.Models.Restaurant;

namespace BookIt.Application.Services
{
    public interface IRestaurantService
    {
        Task<RestaurantModel> AddAsync(RestaurantModelForCreate createRestaurantModel);

        IEnumerable<RestaurantModel> GetAllRestaurant();
    }
}
