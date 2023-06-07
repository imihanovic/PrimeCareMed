using AutoMapper;
using BookIt.Application.Models.Restaurant;
using BookIt.Application.Models.Table;
using BookIt.Application.Models.User;
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

            var objRandom = new Random();
            var uniqueCounter = objRandom.Next(10000, 99999);
            table.Restaurant = restaurant;
            table.Restaurant.Id = restaurant.Id;
            table.TableName = restaurant.RestaurantName + restaurant.City + restaurant.Address+'_'+uniqueCounter;
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

        public IEnumerable<TableModel> TableSorting(IEnumerable<TableModel> tables, string sortOrder)
        {
            IEnumerable<TableModel> sortedtables = tables;
            switch (sortOrder)
            {
                case "TableName":
                    return tables.OrderBy(t => t.TableName);
                case "TableNameDesc":
                    return tables.OrderByDescending(t => t.TableName);
                case "NumberOfSeats":
                    return tables.OrderBy(t => t.NumberOfSeats);
                case "NumberOfSeatsDesc":
                    return tables.OrderByDescending(t => t.NumberOfSeats);
                default:
                    return tables.OrderBy(t => t.TableName);
            }
        }

        public IEnumerable<TableModel> TableFilterByArea(IEnumerable<TableModel> tables, string area)
        {
            IEnumerable<TableModel> filteredtables = tables;
            if (!String.IsNullOrEmpty(area))
            {
                var areaTrim = area.ToLower().Trim();
                filteredtables = tables.Where(t => t.Area.ToLower() == areaTrim);
            }
            return filteredtables;
        }

        public IEnumerable<TableModel> TableFilterBySmoking(IEnumerable<TableModel> tables, string smoking)
        {
            IEnumerable<TableModel> filteredtables = tables;
            if (!String.IsNullOrEmpty(smoking))
            {
                var smokingTrim = smoking.ToLower().Trim();
                filteredtables = tables.Where(t => t.Smoking.ToLower() == smokingTrim);
            }
            return filteredtables;
        }

        public Table EditTableAsync(TableModelForUpdate tableModel)
        {
            var table = _mapper.Map<Table>(tableModel);
            return _tableRepository.UpdateAsync(table).Result;
        }

        public async Task DeleteTableAsync(Guid Id)
        {
            await _tableRepository.DeleteAsync(Id);

        }
    }
}
