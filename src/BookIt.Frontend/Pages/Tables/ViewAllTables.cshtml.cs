using AutoMapper;
using BookIt.Application.Models.Restaurant;
using BookIt.Application.Models.Table;
using BookIt.Application.Services;
using BookIt.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookIt.Frontend.Pages.Tables
{
    public class ViewAllTablesModel : PageModel
    {
        public readonly ITableService _tableService;
        public readonly IRestaurantService _restaurantService;
        public readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;
#nullable enable
        public List<TableModel> Tables { get; set; }

#nullable disable
        public List<string> TableModelProperties;

        [FromRoute]
        public Guid Id { get; set; }

        public RestaurantModel RestaurantModel { get; set; }

        public int TotalPages { get; set; }

        public ViewAllTablesModel(IRestaurantService restaurantService,
            ITableService tableService,
            IRestaurantRepository restaurantRepository,
            IMapper mapper)
        {
            _restaurantService = restaurantService;
            _tableService = tableService;
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
        }

        public void OnGet()
        {
            TableModelProperties = _tableService.GetTableModelFields();

            var restaurant = _restaurantRepository.GetRestaurantByIdAsync(Id).Result;
            RestaurantModel = _mapper.Map<RestaurantModel>(restaurant);
            Tables = _tableService.GetAllTables(Id).ToList();
        }
    }
}
