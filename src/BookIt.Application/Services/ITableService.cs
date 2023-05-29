using BookIt.Application.Models.Table;
using BookIt.Core.Entities;

namespace BookIt.Application.Services
{
    public interface ITableService
    {
        Task<TableModel> AddAsync(TableModelForCreate createTableModel);

        List<string> GetTableModelFields();

        IEnumerable<TableModel> GetAllTables(Guid id);

        IEnumerable<TableModel> TableSorting(IEnumerable<TableModel> tables, string sortOrder);

        IEnumerable<TableModel> TableFilterByArea(IEnumerable<TableModel> tables, string area);

        IEnumerable<TableModel> TableFilterBySmoking(IEnumerable<TableModel> tables, string smoking);

        Table EditTableAsync(TableModelForUpdate tableModel);

        Task DeleteTableAsync(Guid Id);
    }
}
