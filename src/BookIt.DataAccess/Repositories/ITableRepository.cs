using BookIt.Core.Entities;

namespace BookIt.DataAccess.Repositories
{
    public interface ITableRepository
    {
        Task<Table> AddAsync(Table table);

        Task<IEnumerable<Table>> GetAllTablesAsync();

        Task<IEnumerable<Table>> GetAllTablesByRestaurantAsync(Guid restaurantId);

        Task<Table> GetTableByIdAsync(Guid id);

        Task<Table> UpdateAsync(Table table);

        Task DeleteAsync(Guid id);

        Task<Restaurant> GetRestaurantByTableAsync(Guid tableId);
    }
}
