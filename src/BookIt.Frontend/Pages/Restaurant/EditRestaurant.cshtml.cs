using AutoMapper;
using BookIt.Application.Models.Restaurant;
using BookIt.Application.Models.User;
using BookIt.Application.Services;
using BookIt.DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BookIt.Frontend.Pages.Restaurant
{
    [Authorize(Roles = ("Administrator, Manager"))]
    public class EditRestaurantModel : PageModel
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IRestaurantService _restaurantService;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public EditRestaurantModel(IRestaurantRepository restaurantRepository,
            IRestaurantService restaurantService,
            IUserRepository userRepository,
            IUserService userService,
            IMapper mapper)
        {
            _restaurantRepository = restaurantRepository;
            _restaurantService = restaurantService;
            _userService = userService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [FromRoute]
        public Guid Id { get; set; }

        [BindProperty]
        public RestaurantModelForCreate EditRestaurant { get; set; }

        [BindProperty]
        public IEnumerable<ListUsersModel> Managers => _userService.GetAllManagers();
        public void OnGet()
        {
            var restaurant = _restaurantRepository.GetRestaurantByIdAsync(Id).Result;
            //var logedUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            //if(User.IsInRole("Manager") && logedUserId.Value != restaurant.ManagerId)
            //{
            //    return RedirectToPage("/AccessDenied");
            //}
            EditRestaurant = _mapper.Map<RestaurantModelForCreate>(restaurant);
        }
        public IActionResult OnPostEdit()
        {
            if (!ModelState.IsValid) { return Page(); }

            EditRestaurant.Id = Id.ToString();
            try
            {
                _restaurantService.EditRestaurantAsync(EditRestaurant);
                return RedirectToPage("ViewAllRestaurants");
            }
            catch (Exception ex)
            {
                var manager = _userRepository.GetUserByIdAsync(EditRestaurant.ManagerId);
                ViewData["Message"] = string.Format($"Manager {manager.Result.UserName} is already selected for other restaurant!!!");
                Console.WriteLine(ex.Message);
            }

            return Page();
        }

        public IActionResult OnPostDelete()
        {
            _restaurantService.DeleteRestaurantAsync(Id);

            return RedirectToPage("ViewAllRestaurants");
        }
    }
}
