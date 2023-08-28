using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Application.Models.Checkup;
using PrimeCareMed.Application.Models.CheckupAppointment;
using PrimeCareMed.Application.Models.HospitalCheckup;
using PrimeCareMed.Application.Services;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.CheckupAppointment
{
    public class PickCheckupAppointmentDateModel : PageModel
    {
        private readonly ICheckupService _checkupService;
        private readonly IAppointmentService _appointmentService;
        public PickCheckupAppointmentDateModel(
            ICheckupService checkupService, IAppointmentService appointmentService)
        {
            _checkupService = checkupService;
            _appointmentService = appointmentService;
        }
#nullable enable
        public HospitalCheckupModel Checkup { get; set; }
        public AppointmentDetailsModel Appointment { get; set; }
#nullable disable
        public IActionResult OnPost(string selectCheckup, string appointmentId)
        {
            
            var splitString = selectCheckup.Split(',');
            var hospitalId = splitString[0];
            var checkupId = splitString[1];
            ViewData["HospitalId"] = hospitalId;
            ViewData["AppointmentId"] = appointmentId;
            ViewData["CheckupId"] = checkupId;
            Appointment = _appointmentService.GetAppointmentDetailsById(Guid.Parse(appointmentId));
            Checkup = _checkupService.GetHospitalCheckupById(hospitalId, checkupId);
            return Page();
        }
    }
}
