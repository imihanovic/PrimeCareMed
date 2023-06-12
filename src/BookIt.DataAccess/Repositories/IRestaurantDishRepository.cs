using BookIt.Core.Entities;

namespace BookIt.DataAccess.Repositories
{
    public interface IRestaurantDishRepository
    {
        Task<RestaurantDish> AddAsync(RestaurantDish restaurantDish);

        Task<IEnumerable<RestaurantDish>> GetAllRestaurantDishesAsync(string restaurantId);

        Task<RestaurantDish> GetRestaurantDishByIdAsync(Guid id);

        Task<RestaurantDish> UpdateAsync(RestaurantDish restaurantDish);
    }
}
