using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Application.Models.CheckupAppointment;
using PrimeCareMed.Application.Models.HospitalCheckup;
using PrimeCareMed.Application.Services;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.CheckupAppointment
{
    [Authorize(Roles = "Doctor, SysAdministrator")]
    public class PickCheckupAppointmentTimeModel : PageModel
    {
        private readonly ICheckupService _checkupService;
        private readonly ICheckupAppointmentService _checkupAppointmentService;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IAppointmentService _appointmentService;
        public PickCheckupAppointmentTimeModel(
            ICheckupService checkupService,
            ICheckupAppointmentService checkupAppointmentService,
            IAppointmentRepository appointmentRepository,
            IAppointmentService appointmentService)
        {
            _checkupService = checkupService;
            _checkupAppointmentService = checkupAppointmentService;
            _appointmentRepository = appointmentRepository;
            _appointmentService = appointmentService;
        }
        [BindProperty]
        public CheckupAppointmentModelForCreate NewCheckupAppointment { get; set; }

#nullable enable
        public List<DateTime>? AvailableSlots { get; set; }
        public string? time { get; set; }
        public HospitalCheckupModel Checkup { get; set; }
        public AppointmentDetailsModel Appointment { get; set; }
#nullable disable
        public async Task<IActionResult> OnPostAsync(string hospitalId, string checkupId, string checkupDate, string appointmentId)
        {
            var appointment = _appointmentRepository.GetAppointmentByIdAsync(Guid.Parse(appointmentId)).Result;
            var availableSlots = _checkupService.GetAvailableTimeslotsForCheckup(checkupDate, checkupId, hospitalId);
            AvailableSlots = availableSlots;
            
            ViewData["HospitalId"] = hospitalId;
            ViewData["CheckupId"] = checkupId;
            ViewData["CheckupDate"] = checkupDate;
            ViewData["AppointmentId"] = appointmentId;
            var currentdate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd");
            var currentHour = int.Parse(DateTime.Now.ToLocalTime().ToString("HH")) + 2;
            Appointment = _appointmentService.GetAppointmentDetailsById(Guid.Parse(appointmentId));
            Checkup = _checkupService.GetHospitalCheckupById(hospitalId, checkupId);
            if(NewCheckupAppointment.Time.ToString("dd/MM/yyyy HH:mm:ss") != "01/01/0001 00:00:00")
            {
                try
                {
                    NewCheckupAppointment.Date = DateTime.Parse(checkupDate).ToUniversalTime();
                    NewCheckupAppointment.AppointmentId = Guid.Parse(appointmentId);
                    NewCheckupAppointment.HospitalId = Guid.Parse(hospitalId);
                    NewCheckupAppointment.CheckupId = Guid.Parse(checkupId);
                    NewCheckupAppointment.CheckupStatus = Core.Enums.CheckupStatus.Active;
                    await _checkupAppointmentService.AddCheckupAppointment(NewCheckupAppointment);
                    return RedirectToPage("/Appointment/ViewAppointmentDetails", new {Id = Guid.Parse(appointmentId)});
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return Page();
        }
    }
}
