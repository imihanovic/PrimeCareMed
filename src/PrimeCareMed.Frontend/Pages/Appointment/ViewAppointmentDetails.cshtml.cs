using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Application.Models.MedicalReport;
using PrimeCareMed.Application.Models.Patient;
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
        public readonly IMedicalReportService _reportService;
        public readonly IMedicalReportRepository _reportRepository;
        public readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public AppointmentDetailsModel Appointment { get; set; }
#nullable enable
        [BindProperty]
        public MedicalReportModel? MedicalReport { get; set; }
#nullable disable

        [FromRoute]
        public Guid Id { get; set; }
        public int TotalPages { get; set; }

        public ViewAppointmentDetailsModel(IAppointmentService appointmentService,
            UserManager<ApplicationUser> userManager,
            IAppointmentRepository appointmentRepository,
            IMedicalReportService reportService,
            IMedicalReportRepository reportRepository
            )
        {
            _appointmentService = appointmentService;
            _userManager = userManager;
            _appointmentRepository = appointmentRepository;
            _reportService = reportService;
            _reportRepository = reportRepository;

        }
        public void OnGet()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            Appointment = _appointmentService.GetAppointmentDetailsById(Id);
            if(_reportRepository.CheckIfReportForAppointmentExists(Id))
            {
                MedicalReport = _reportService.GetReportForAppointment(Id);
            }
            if (currentUserRole == "Doctor")
            {
                var appointmentDB = _appointmentRepository.GetAppointmentByIdAsync(Id).Result;
                appointmentDB.Status = Core.Enums.AppointmentStatus.Pending;
                _appointmentRepository.UpdateAsync(appointmentDB);
            }    
        }
        public async Task<IActionResult> OnPostReport(string description)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var a = new MedicalReportModelForCreate();
                a.Description = description;
                await _reportService.AddAsync(a, Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Page();
        }
    }
}
