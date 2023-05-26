using BookIt.Application.Models.Table;
using BookIt.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookIt.Frontend.Pages.Tables
{
    public class ViewAllTablesModel : PageModel
    {
        public readonly ITableService _tableService;
        public readonly IRestaurantService _restaurantService;
#nullable enable
        public List<TableModel> Tables { get; set; }

#nullable disable
        public List<string> TableModelProperties;

        public int TotalPages { get; set; }

        public ViewAllTablesModel(IRestaurantService restaurantService,
            ITableService tableService)
        {
            _restaurantService = restaurantService;
            _tableService = tableService;
        }

        public void OnGet()
        {
            TableModelProperties = _tableService.GetTableModelFields();

            Tables = _tableService.GetAllTables().ToList();
        }
    }
}
