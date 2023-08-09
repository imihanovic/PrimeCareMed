using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;
using PrimeCareMed.DataAccess.Repositories.Impl;

namespace PrimeCareMed.Frontend.Pages.Appointment
{
    public class ViewAllAppointmentsModel : PageModel
    {
        public readonly IAppointmentService _appointmentService;
        public readonly IAppointmentRepository _appointmentRepository;
        public readonly UserManager<ApplicationUser> _userManager;

        public List<string> AppointmentModelProperties;
        public PaginatedList<AppointmentModel> Appointments { get; set; }
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
        public IActionResult OnGet(string sort, string currentFilter, string keyword, string dateFilter, int? pageIndex)

        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();

            AppointmentModelProperties = _appointmentService.GetAppointmentModelFields();
            var cookie = Request.Cookies["sessionCookie"];

            if (currentUserRole == "Doctor" && cookie is null)
            {
                return Redirect("/Shift/CreateShift");
            }
            else if (currentUserRole == "Nurse" && cookie is null)
            {
                return Redirect("/Shift/CreateShift");
            }
            var appointments = new List<AppointmentModel>();
            if (currentUserRole == "Doctor" || currentUserRole == "Nurse")
            {
                appointments = _appointmentService.GetAllAppointmentsForDoctor(cookie).ToList();
            }
            else
            {
                appointments = _appointmentService.GetAllAppointments().ToList();
            }
            //Appointments = appointments;
            

            if (keyword != null)
            {
                pageIndex = 1;
            }
            else
            {
                keyword = currentFilter;
            }

            ViewData["CurrentFilter"] = keyword;
            int pageSize = 7;

            ViewData["CurrentSort"] = sort;

            appointments = _appointmentService.AppointmentSorting(appointments, sort).ToList();

            ViewData["Keyword"] = keyword;
            appointments = _appointmentService.AppointmentSearch(appointments, keyword).ToList();

            ViewData["DateFilter"] = dateFilter;
            appointments = _appointmentService.AppointmentFilterDate(appointments, dateFilter).ToList();

            TotalPages = (int)Math.Ceiling(decimal.Divide(appointments.Count(), pageSize));            
            Appointments = PaginatedList<AppointmentModel>.Create(appointments, pageIndex ?? 1, pageSize);

            return Page();
        }
    }
}
