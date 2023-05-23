using BookIt.Core.Entities;

namespace BookIt.DataAccess.Repositories
{
    public interface IRestaurantRepository
    {
        Task AddAsync(Restaurant restaurant);

        Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
    }
}
