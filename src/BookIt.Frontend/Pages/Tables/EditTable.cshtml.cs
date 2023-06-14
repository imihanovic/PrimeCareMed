using AutoMapper;
using BookIt.Application.Models.Table;
using BookIt.Application.Services;
using BookIt.Core.Entities;
using BookIt.Core.Entities.Identity;
using BookIt.DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookIt.Frontend.Pages.Tables
{
    [Authorize(Roles = "Manager")]
    public class EditTableModel : PageModel
    {
        private readonly ITableRepository _tableRepository;
        private readonly ITableService _tableService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public EditTableModel(ITableRepository tableRepository,
            ITableService tableService,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _tableRepository = tableRepository;
            _tableService = tableService;
            _userManager = userManager;
            _mapper = mapper;
        }

        [FromRoute]
        public Guid Id { get; set; }

        [BindProperty]
        public TableModelForUpdate EditTable { get; set; }

        public Table Table { get; set; }

        public IActionResult OnGet()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            var table = _tableRepository.GetTableByIdAsync(Id).Result;
            var restaurantName = _tableRepository.GetRestaurantByTableAsync(Id).Result.RestaurantName;
            if(currentUserRole == "Manager" && currentUser.Restaurant is not null)
            {
                var managerTables = currentUser.Restaurant.Tables.ToList();
                if (currentUser.Restaurant is not null && !managerTables.Contains(table))
                {

                    return RedirectToPage("../Restaurant/ViewAllRestaurants");
                }
            }
            ViewData["TableName"] = table.TableName;
            ViewData["RestaurantName"] = restaurantName;
            ViewData["RestaurantId"] = Id;
            EditTable = _mapper.Map<TableModelForUpdate>(table);
            return Page();
        }
        public IActionResult OnPostEdit()
        {
            if (!ModelState.IsValid) { return Page(); }

            EditTable.Id = Id.ToString();

            var restaurantId = _tableRepository.GetRestaurantByTableAsync(Id).Result.Id.ToString();

            _tableService.EditTableAsync(EditTable);

            return RedirectToPage("ViewAllTables", new { id = restaurantId });
        }

        public IActionResult OnPostDelete()
        {
            var restaurantId = _tableRepository.GetRestaurantByTableAsync(Id).Result.Id.ToString();

            _tableService.DeleteTableAsync(Id);

            return RedirectToPage("ViewAllTables", new { id = restaurantId });
        }
    }
}
