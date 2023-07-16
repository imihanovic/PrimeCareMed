using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.Appointment
{
    public class ViewAllAppointmentsModel : PageModel
    {
        public readonly IAppointmentService _appointmentService;
        public readonly IAppointmentRepository _appointmentRepository;
        public readonly UserManager<ApplicationUser> _userManager;

        public List<string> AppointmentModelProperties;
        public List<AppointmentModel> Appointments { get; set; }
        public int TotalPages { get; set; }

        public ViewAllAppointmentsModel(IAppointmentService appointmentService,
            UserManager<ApplicationUser> userManager,
            IAppointmentRepository appointmentRepository
            )
        {
            _appointmentService = appointmentService;
            _userManager = userManager;
            _appointmentRepository = appointmentRepository;

        }
        public void OnGet()

        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            AppointmentModelProperties = _appointmentService.GetAppointmentModelFields();
            var appointments = new List<AppointmentModel>();
            var cookie = Request.Cookies["shift"];
            if (currentUserRole == "Doctor" || currentUserRole == "Nurse")
            {
                appointments = (List<AppointmentModel>)_appointmentService.GetAllAppointmentsForDoctor(cookie);
            }
            else
            {
                appointments = (List<AppointmentModel>)_appointmentService.GetAllAppointments();
            }
            Appointments = appointments;
            //ViewData["Keyword"] = keyword;
            //dishes = _dishService.DishSearch(dishes, keyword);

            //ViewData["CategoryFilter"] = categoryFilter;
            //dishes = _dishService.DishFilter(dishes, categoryFilter);

            //Dishes = PaginatedList<DishModel>.Create(dishes, pageIndex ?? 1, pageSize);

            //TotalPages = (int)Math.Ceiling(decimal.Divide(dishes.Count(), pageSize));

        }
    }
}
