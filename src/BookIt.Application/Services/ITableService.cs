using BookIt.Application.Models.Table;

namespace BookIt.Application.Services
{
    public interface ITableService
    {
        Task<TableModel> AddAsync(TableModelForCreate createTableModel);

        List<string> GetTableModelFields();

        IEnumerable<TableModel> GetAllTables(Guid id);
    }
}
