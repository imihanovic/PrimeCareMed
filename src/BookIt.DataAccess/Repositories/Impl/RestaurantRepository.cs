using BookIt.Core.Entities;
using BookIt.Core.Entities.Identity;
using BookIt.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookIt.DataAccess.Repositories.Impl
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly DatabaseContext _context;

        public RestaurantRepository(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(Restaurant restaurant)
        {
            await _context.Restaurants.AddAsync(restaurant);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
        {
            return await _context.Restaurants.OrderBy(r => r.Id).ToListAsync();
        }
    }
}
