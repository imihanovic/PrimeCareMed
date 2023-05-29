using BookIt.DataAccess.Persistence;
using BookIt.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookIt.DataAccess.Repositories.Impl
{
    public class DishRepository : IDishRepository
    {
        private readonly DatabaseContext _context;
        public DishRepository(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
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
        public async Task DeleteAsync(Guid id)
        {
            var deleteItem = _context.Dishes.FirstOrDefault(r => r.Id == id);
            _context.Dishes.Remove(deleteItem);
            await _context.SaveChangesAsync();
        }
    }
}
