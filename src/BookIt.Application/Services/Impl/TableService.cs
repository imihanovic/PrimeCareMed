using AutoMapper;
using BookIt.Application.Models.Restaurant;
using BookIt.Application.Models.Table;
using BookIt.Core.Entities;
using BookIt.DataAccess.Repositories;

namespace BookIt.Application.Services.Impl
{
    public class TableService : ITableService
    {
        private readonly IMapper _mapper;
        private readonly ITableRepository _tableRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IUserRepository _userRepository;

        public TableService(IMapper mapper,
            ITableRepository tableRepository,
            IUserRepository userRepository,
            IRestaurantRepository restaurantRepository)
        {
            _mapper = mapper;
            _tableRepository = tableRepository;
            _userRepository = userRepository;
            _restaurantRepository = restaurantRepository;
        }

        public async Task<TableModel> AddAsync(TableModelForCreate createTableModel)
        {
            var table = _mapper.Map<Table>(createTableModel);
            var restaurant = _restaurantRepository.GetRestaurantByIdAsync(Guid.Parse(createTableModel.RestaurantId)).Result;
            var counter = _tableRepository.GetAllTablesAsync().Result.Where(t => t.Restaurant == restaurant).Count()+1;
            table.Restaurant = restaurant;
            table.TableName = restaurant.RestaurantName + restaurant.City + restaurant.Address+counter;
            await _tableRepository.AddAsync(table);
            return _mapper.Map<TableModel>(table);
        }

        public List<string> GetTableModelFields()
        {
            var tableDto = new TableModel();
            return tableDto.GetType().GetProperties().Where(x => x.Name != "RestaurantId" && x.Name != "Id").Select(x => x.Name).ToList();
        }

        public IEnumerable<TableModel> GetAllTables()
        {
            var tablesFromDatabase = _tableRepository.GetAllTablesAsync().Result;

            List<TableModel> tables = new List<TableModel>();
            foreach (var table in tablesFromDatabase)
            {
                var tableDto = _mapper.Map<TableModel>(table);
                tables.Add(tableDto);
            }
            return tables.AsEnumerable();
        }
    }
}
