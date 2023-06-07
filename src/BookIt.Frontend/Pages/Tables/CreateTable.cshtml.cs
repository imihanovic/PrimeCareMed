using AutoMapper;
using BookIt.Application.Models.Restaurant;
using BookIt.Application.Models.Table;
using BookIt.Application.Models.User;
using BookIt.Application.Services;
using BookIt.Core.Entities;
using BookIt.Core.Entities.Identity;
using BookIt.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookIt.Frontend.Pages.Tables
{
    public class CreateTableModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ITableService _tableService;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IRestaurantService _restaurantService;
        private readonly IMapper _mapper;
        public CreateTableModel(UserManager<ApplicationUser> userManager,
            IMapper mapper,
            ITableService tableService,
            IRestaurantRepository restaurantRepository,
            IRestaurantService restaurantService,
            IUserRepository userRepository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _restaurantRepository = restaurantRepository;
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
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();

            if(currentUserRole == "Manager")
            {
                var restaurant = _restaurantRepository.GetRestaurantByManagerIdAsync(currentUser.Id).Result;
                Console.WriteLine($"restoran id : {restaurant.Id}");
                NewTable.RestaurantId = restaurant.Id.ToString();
            }

            await _tableService.AddAsync(NewTable);

            var restaurantId = NewTable.RestaurantId;
            
            return RedirectToPage("ViewAllTables", new { id = restaurantId });
        }
    }
}
