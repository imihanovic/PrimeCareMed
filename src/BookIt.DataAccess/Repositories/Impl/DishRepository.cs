using BookIt.DataAccess.Persistence;
using BookIt.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookIt.DataAccess.Repositories.Impl
{
    public class DishRepository : IDishRepository
    {
        private readonly DatabaseContext _context;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IRestaurantDishRepository _restaurantDishRepository;
        public DishRepository(DatabaseContext context, IRestaurantRepository restaurantRepository, IRestaurantDishRepository restaurantDishRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _restaurantRepository = restaurantRepository;
            _restaurantDishRepository = restaurantDishRepository;
        }

        public async Task<Dish> AddAsync(Dish dish)
        {
            await _context.Dishes.AddAsync(dish);
            await _context.SaveChangesAsync();
            return dish;
        }

        public async Task<IEnumerable<Dish>> GetAllDishesAsync()
        {
            return await _context.Dishes.OrderBy(r => r.Id).ToListAsync();
        }

        public async Task<IEnumerable<Dish>> GetAllDishesForRestaurantAsync(string restaurantId)
        {
            var restaurantDishes = _restaurantDishRepository.GetAllRestaurantDishesAsync(restaurantId).Result.Select(r => r.Dish);
            var allDishes = await GetAllDishesAsync();
            var filteredDishes = allDishes.Where(x => restaurantDishes.All(y => y.Id != x.Id));
            return filteredDishes;
        }
        public async Task DeleteAsync(Guid id)
        {
            var deleteItem = _context.Dishes.FirstOrDefault(r => r.Id == id);
            _context.Dishes.Remove(deleteItem);
            await _context.SaveChangesAsync();
        }
    }
}
