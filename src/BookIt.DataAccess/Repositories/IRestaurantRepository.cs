using BookIt.Core.Entities;

namespace BookIt.DataAccess.Repositories
{
    public interface IRestaurantRepository
    {
        Task<Restaurant> AddAsync(Restaurant restaurant);

        Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();

        Task<Restaurant> UpdateAsync(Restaurant restaurant);

        Task DeleteAsync(Guid id);

        Task<Restaurant> GetRestaurantByIdAsync(Guid id);
    }
}
