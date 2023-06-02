using BookIt.Application.Models.Dish;
using BookIt.Application.Models.Restaurant;
using BookIt.Core.Entities;

namespace BookIt.Application.Services
{
    public interface IDishService
    {
        Task<DishModel> AddAsync(DishModelForCreate createDishModel);

        IEnumerable<DishModel> GetAllDishes();

        List<string> GetDishModelFields();

        IEnumerable<DishModel> DishSearch(IEnumerable<DishModel> dishes, string searchString);

        IEnumerable<DishModel> DishFilter(IEnumerable<DishModel> dishes, string category);

        Task DeleteDishAsync(Guid Id);
    }
}
