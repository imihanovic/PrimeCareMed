using BookIt.Core.Entities;
using BookIt.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookIt.DataAccess.Repositories.Impl
{
    public class TableRepository : ITableRepository
    {
        private readonly DatabaseContext _context;
        private readonly IRestaurantRepository _restaurantRepository;
        public TableRepository(DatabaseContext context, IRestaurantRepository restaurantRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _restaurantRepository = restaurantRepository;
        }

        public async Task<Table> AddAsync(Table table)
        {
            await _context.Tables.AddAsync(table);
            await _context.SaveChangesAsync();
            return table;
        }

        public async Task<IEnumerable<Table>> GetAllTablesAsync()
        {
            return await _context.Tables.OrderBy(r => r.Id).Include(r => r.Restaurant).ToListAsync();
        }

        public async Task<IEnumerable<Table>> GetAllTablesByRestaurantAsync(Guid restaurantId)
        {
            return await _context.Tables.OrderBy(r => r.Id).Include(r => r.Restaurant).Include(r => r.Reservations).Where(t => t.Restaurant.Id == restaurantId).ToListAsync();
        }

        public async Task<Table> UpdateAsync(Table table)
        {
            var editItem = await GetTableByIdAsync(table.Id);
            editItem.Area = table.Area;
            editItem.Smoking = table.Smoking;
            await _context.SaveChangesAsync();
            return editItem;
        }

        public async Task DeleteAsync(Guid id)
        {
            var deleteItem = _context.Tables.FirstOrDefault(t => t.Id == id);
            _context.Tables.Remove(deleteItem);
            await _context.SaveChangesAsync();
        }
        public async Task<Table> GetTableByIdAsync(Guid id)
        {
            return await _context.Tables.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Restaurant> GetRestaurantByTableAsync(Guid tableId)
        {
            var allRestaurants = _restaurantRepository.GetAllRestaurantsAsync().Result;
            var table = GetTableByIdAsync(tableId).Result;
            var restaurant = allRestaurants.First(r => r.Tables.Contains(table));
            //var restaurant1 = await _restaurantRepository.GetRestaurantByIdAsync(table.Restaurant.Id);
            return restaurant;
        }
    }
}
