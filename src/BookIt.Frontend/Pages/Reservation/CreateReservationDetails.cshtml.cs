using AutoMapper;
using BookIt.Application.Services;
using BookIt.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookIt.Frontend.Pages.Reservation
{
    public class CreateReservationDetailsModel : PageModel
    {
        private readonly IRestaurantService _restaurantService;

        public CreateReservationDetailsModel(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [FromRoute]
        public Guid Id { get; set; }


        public void OnGet()
        {
            var restaurant = _restaurantService.GetAllRestaurants().FirstOrDefault(r => r.Id == Id);
            if(restaurant != null)
            {
                ViewData["RestaurantName"] = restaurant.RestaurantName;
                ViewData["Address"] = restaurant.Address;
            }
        }

        public IActionResult OnPost(int numberOfPersons, string tableArea, string smokingArea, string reservationDate)
        {
            return RedirectToPage("CreateReservation", new { numberOfPersons = numberOfPersons, tableArea = tableArea, smokingArea = smokingArea, reservationDate = reservationDate, restaurantId = Id});
        }
    }
}
