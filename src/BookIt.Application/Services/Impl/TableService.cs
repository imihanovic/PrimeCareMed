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
            table.Restaurant.Id = restaurant.Id;
            table.TableName = restaurant.RestaurantName + restaurant.City + restaurant.Address+'_'+counter;
            await _tableRepository.AddAsync(table);
            return _mapper.Map<TableModel>(table);
        }

        public List<string> GetTableModelFields()
        {
            var tableDto = new TableModel();
            return tableDto.GetType().GetProperties().Where(x => x.Name != "RestaurantId" && x.Name != "Id" && x.Name != "City" && x.Name != "Address" && x.Name != "RestaurantOwner" && x.Name != "RestaurantName").Select(x => x.Name).ToList();
        }

        public IEnumerable<TableModel> GetAllTables(Guid id)
        {
            var tablesFromDatabase = _tableRepository.GetAllTablesAsync().Result;

            List<TableModel> tables = new List<TableModel>();
            foreach (var table in tablesFromDatabase)
            {
                var tableDto = _mapper.Map<TableModel>(table);
                tableDto.RestaurantId = table.Restaurant.Id.ToString();
                tableDto.RestaurantOwner = table.Restaurant.RestaurantOwner;
                tableDto.RestaurantName = table.Restaurant.RestaurantName;
                tableDto.City = table.Restaurant.City;
                tableDto.Address = table.Restaurant.Address;
                if (table.Restaurant.Id  == id)
                {                  
                    tables.Add(tableDto);
                }
            }
            return tables.AsEnumerable();
        }
    }
}
