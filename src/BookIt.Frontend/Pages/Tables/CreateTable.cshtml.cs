using AutoMapper;
using BookIt.Application.Models.Restaurant;
using BookIt.Application.Models.Table;
using BookIt.Application.Models.User;
using BookIt.Application.Services;
using BookIt.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookIt.Frontend.Pages.Tables
{
    public class CreateTableModel : PageModel
    {
        private readonly ITableRepository _tableRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITableService _tableService;
        private readonly IRestaurantService _restaurantService;
        private readonly IMapper _mapper;
        public CreateTableModel(ITableRepository tableRepository,
            IMapper mapper,
            ITableService tableService,
            IRestaurantService restaurantService,
            IUserRepository userRepository)
        {
            _tableRepository = tableRepository;
            _mapper = mapper;
            _restaurantService = restaurantService;
            _userRepository = userRepository;
            _tableService = tableService;
        }

        [BindProperty]
        public TableModelForCreate NewTable{ get; set; }

        [BindProperty]
        public IEnumerable<RestaurantModel> Restaurants => _restaurantService.GetAllRestaurants();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { return Page(); }

            await _tableService.AddAsync(NewTable);
            return RedirectToPage("ViewAllTables");
        }
    }
}
