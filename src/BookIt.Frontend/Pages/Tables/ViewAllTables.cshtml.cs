using AutoMapper;
using BookIt.Application.Models.Restaurant;
using BookIt.Application.Models.Table;
using BookIt.Application.Models.User;
using BookIt.Application.Services;
using BookIt.Application.Services.Impl;
using BookIt.Core.Entities;
using BookIt.Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using BookIt.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookIt.Frontend.Pages.Tables
{
    [Authorize(Roles = "Administrator,Manager")]
    public class ViewAllTablesModel : PageModel
    {
        public readonly ITableService _tableService;
        public readonly IRestaurantService _restaurantService;
        public readonly IRestaurantRepository _restaurantRepository;
        public readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
#nullable enable
        public PaginatedList<TableModel> Tables { get; set; }

#nullable disable
        public List<string> TableModelProperties;

        [FromRoute]
        public Guid Id { get; set; }

        public IEnumerable<TableModel> AllTables { get; set; }
        public RestaurantModel RestaurantModel { get; set; }

        public int TotalPages { get; set; }

        public ViewAllTablesModel(IRestaurantService restaurantService,
            ITableService tableService,
            IRestaurantRepository restaurantRepository,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _restaurantService = restaurantService;
            _tableService = tableService;
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public IActionResult OnGet(string sort, string areaFilter, string smokingFilter, int? pageIndex)
        {

            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();

            TableModelProperties = _tableService.GetTableModelFields();

            int pageSize = 1;

            var restaurant = _restaurantRepository.GetRestaurantByIdAsync(Id).Result;

            RestaurantModel = _mapper.Map<RestaurantModel>(restaurant);

            if (currentUser.Restaurant != restaurant && currentUserRole == "Manager")
            {
                return RedirectToPage("../Restaurant/ViewAllRestaurants");
            }
            
            var tables = _tableService.GetAllTables(Id);

            AllTables = tables;

            ViewData["CurrentSort"] = sort;
            tables = _tableService.TableSorting(tables, sort);

            ViewData["AreaFilter"] = areaFilter;
            tables = _tableService.TableFilterByArea(tables, areaFilter);

            ViewData["SmokingFilter"] = smokingFilter;
            tables = _tableService.TableFilterBySmoking(tables, smokingFilter);

            Tables = PaginatedList<TableModel>.Create(tables, pageIndex ?? 1, pageSize);

            TotalPages = (int)Math.Ceiling(decimal.Divide(tables.Count(), pageSize));

            return Page();
        }
    }
}
