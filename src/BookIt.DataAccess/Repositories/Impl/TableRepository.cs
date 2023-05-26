using BookIt.Core.Entities;
using BookIt.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookIt.DataAccess.Repositories.Impl
{
    public class TableRepository : ITableRepository
    {
        private readonly DatabaseContext _context;

        public TableRepository(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Table> AddAsync(Table table)
        {
            await _context.Tables.AddAsync(table);
            await _context.SaveChangesAsync();
            return table;
        }

        public async Task<IEnumerable<Table>> GetAllTablesAsync()
        {
            return await _context.Tables.OrderBy(r => r.Id).ToListAsync();
        }
    }
}
