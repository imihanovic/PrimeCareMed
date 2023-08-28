using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.Appointment
{
    [Authorize(Roles = "Doctor, Nurse")]
    public class WaitingRoomModel : PageModel
    {
        public readonly IAppointmentService _appointmentService;
        public readonly IAppointmentRepository _appointmentRepository;
        public readonly UserManager<ApplicationUser> _userManager;

        public List<string> AppointmentModelProperties;
        public PaginatedList<AppointmentModel> Appointments { get; set; }
        public int TotalPages { get; set; }

        public WaitingRoomModel(IAppointmentService appointmentService,
            UserManager<ApplicationUser> userManager,
            IAppointmentRepository appointmentRepository
            )
        {
            _appointmentService = appointmentService;
            _userManager = userManager;
            _appointmentRepository = appointmentRepository;

        }
        public IActionResult OnGet(string sort, string currentFilter, string keyword, string statusFilter, int? pageIndex)

        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            if(currentUser is null)
            {
                return Redirect("/Identity/Account/Login");
            }
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            AppointmentModelProperties = _appointmentService.GetAppointmentModelFields();
            var appointments = new List<AppointmentModel>();
            var cookie = Request.Cookies["sessionCookie"];
            if (currentUserRole == "Doctor" && cookie is null)
            {
                return Redirect("/Shift/CreateShift");
            }
            else if (currentUserRole == "Nurse" && cookie is null)
            {
                return Redirect("/Shift/CreateShift");
            }
            if (cookie != null)
            {
                appointments = _appointmentService.GetAllAppointmentsInWaitingRoom(cookie).ToList();
            }
            if (keyword != null)
            {
                pageIndex = 1;
            }
            else
            {
                keyword = currentFilter;
            }
            int pageSize = 7;

            ViewData["CurrentSort"] = sort;
            if(sort != "")
            {
                appointments = _appointmentService.AppointmentSorting(appointments, sort).ToList();
            }

            ViewData["Keyword"] = keyword;
            appointments = _appointmentService.AppointmentSearch(appointments, keyword).ToList();

            ViewData["StatusFilter"] = statusFilter;
            appointments = _appointmentService.AppointmentFilterStatus(appointments, statusFilter).ToList();

            TotalPages = (int)Math.Ceiling(decimal.Divide(appointments.Count(), pageSize));
            Appointments = PaginatedList<AppointmentModel>.Create(appointments, pageIndex ?? 1, pageSize);
            return Page();
        }
    }
}
