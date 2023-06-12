using AutoMapper;
using BookIt.Application.Services;
using BookIt.Core.Entities.Identity;
using BookIt.DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace BookIt.Frontend.Pages.Reservation
{
    [Authorize(Roles = "Manager,Customer")]
    public class CreateReservationDetailsModel : PageModel
    {
        private readonly IRestaurantService _restaurantService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateReservationDetailsModel(IRestaurantService restaurantService, UserManager<ApplicationUser> userManager)
        {
            _restaurantService = restaurantService;
            _userManager = userManager;
        }

        [FromRoute]
        public Guid Id { get; set; }


        public IActionResult OnGet()
        {
            var restaurant = _restaurantService.GetAllRestaurants().FirstOrDefault(r => r.Id == Id);
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            if (restaurant != null)
            {
                ViewData["RestaurantName"] = restaurant.RestaurantName;
                ViewData["Address"] = restaurant.Address;
                if (currentUserRole == "Manager" && (currentUser.Restaurant != null && restaurant != null && currentUser.Restaurant.Id.ToString() != restaurant.Id.ToString()))
                {
                    return RedirectToPage("../Restaurant/ViewAllRestaurants");
                }
            }
            return Page();
            
        }
    }
}
