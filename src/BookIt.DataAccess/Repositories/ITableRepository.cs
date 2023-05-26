using BookIt.Core.Entities;

namespace BookIt.DataAccess.Repositories
{
    public interface ITableRepository
    {
        Task<Table> AddAsync(Table table);

        Task<IEnumerable<Table>> GetAllTablesAsync();
    }
}
