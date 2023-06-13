using BookIt.Core.Entities;
using BookIt.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookIt.DataAccess.Repositories.Impl
{
    public class RestaurantDishRepository : IRestaurantDishRepository
    {
        private readonly DatabaseContext _context;
        public RestaurantDishRepository(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<RestaurantDish> AddAsync(RestaurantDish restaurantDish)
        {
            await _context.RestaurantDishes.AddAsync(restaurantDish);
            await _context.SaveChangesAsync();
            return restaurantDish;
        }

        public async Task<IEnumerable<RestaurantDish>> GetAllRestaurantDishesAsync(string restaurantId)
        {
            return await _context.RestaurantDishes.Include(r => r.Restaurant).Include(r => r.Dish).Where(r => r.Restaurant.Id.ToString() == restaurantId).OrderBy(r => r.Id).ToListAsync();
        }

        public async Task<RestaurantDish> GetRestaurantDishByIdAsync(Guid id)
        {
            return await _context.RestaurantDishes.Include(r => r.Restaurant).Include(r => r.Dish).FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<RestaurantDish> UpdateAsync(RestaurantDish restaurantDish)
        {
            var editItem = await GetRestaurantDishByIdAsync(restaurantDish.Id);
            editItem.IsAvailable = restaurantDish.IsAvailable;
            editItem.Price = restaurantDish.Price;
            await _context.SaveChangesAsync();
            return editItem;
        }

        public async Task DeleteAsync(Guid id)
        {
            var deleteItem = _context.RestaurantDishes.FirstOrDefault(r => r.Id == id);
            _context.RestaurantDishes.Remove(deleteItem);
            await _context.SaveChangesAsync();
        }
    }
}
