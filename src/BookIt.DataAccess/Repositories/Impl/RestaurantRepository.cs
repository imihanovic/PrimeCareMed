using BookIt.Core.Entities;
using BookIt.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookIt.DataAccess.Repositories.Impl
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly DatabaseContext _context;
        private readonly IUserRepository _userRepository;

        public RestaurantRepository(DatabaseContext context, IUserRepository userRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userRepository = userRepository;
        }

        public async Task<Restaurant> AddAsync(Restaurant restaurant)
        {
            await _context.Restaurants.AddAsync(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }
        public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
        {
            return await _context.Restaurants.OrderBy(r => r.Id).Include(r => r.Tables).ToListAsync();
        }

        public async Task<Restaurant> UpdateAsync(Restaurant restaurant)
        {
            var editItem = await GetRestaurantByIdAsync(restaurant.Id);
            editItem.RestaurantOwner = restaurant.RestaurantOwner;
            editItem.RestaurantName = restaurant.RestaurantName;
            editItem.City = restaurant.City;
            editItem.Address = restaurant.Address;
            editItem.ManagerId = restaurant.ManagerId;
            await _context.SaveChangesAsync();
            return editItem;
        }

        public async Task DeleteAsync(Guid id)
        {
            var deleteItem = _context.Restaurants.FirstOrDefault(r => r.Id == id);
            _context.Restaurants.Remove(deleteItem);
            await _context.SaveChangesAsync();
        }
        public async Task<Restaurant> GetRestaurantByIdAsync(Guid id)
        {
            return await _context.Restaurants.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Restaurant> GetRestaurantByManagerIdAsync(string managerId)
        {
            var allRestaurants = await GetAllRestaurantsAsync();
            var restaurant = allRestaurants.First(r => r.ManagerId == managerId);
            return restaurant;
        }
    }
}
