using BookIt.Core.Entities;

namespace BookIt.DataAccess.Repositories
{
    public interface IDishRepository
    {
        Task<Dish> AddAsync(Dish dish);

        Task<IEnumerable<Dish>> GetAllDishesAsync();

        Task DeleteAsync(Guid id);
    }
}
