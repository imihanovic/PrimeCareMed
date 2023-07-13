using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.Appointment
{
    [Authorize(Roles = "Doctor, Nurse, SysAdministrator")]
    public class ViewAppointmentDetailsModel : PageModel
    {
        public readonly IAppointmentService _appointmentService;
        public readonly IAppointmentRepository _appointmentRepository;
        public readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public AppointmentDetailsModel Appointment { get; set; }

        [FromRoute]
        public Guid Id { get; set; }
        public int TotalPages { get; set; }

        public ViewAppointmentDetailsModel(IAppointmentService appointmentService,
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
            Appointment = _appointmentService.GetAppointmentDetailsById(Id);
            if (currentUserRole == "Doctor")
            {
                var appointmentDB = _appointmentRepository.GetAppointmentByIdAsync(Id).Result;
                appointmentDB.Status = Core.Enums.AppointmentStatus.Pending;
                _appointmentRepository.UpdateAsync(appointmentDB);
            }

            
        }
    }
}
