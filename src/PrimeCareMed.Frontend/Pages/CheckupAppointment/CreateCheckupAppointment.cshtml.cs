using AutoMapper;
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
    public class CreateCheckupAppointmentModel : PageModel
    {
        private readonly ICheckupService _checkupService;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IAppointmentService _appointmentService;
        public CreateCheckupAppointmentModel(
            ICheckupService checkupService,
            IAppointmentRepository appointmentRepository,
            IAppointmentService appointmentService)
        {
            _checkupService = checkupService;
            _appointmentRepository = appointmentRepository;
            _appointmentService = appointmentService;
        }
        [FromRoute]
        public Guid Id { get; set; }

        [BindProperty]
        public CheckupAppointmentModelForCreate NewCheckupAppointment { get; set; }
#nullable enable
        public IEnumerable<HospitalCheckupModel>? Checkups { get; set; }
        public AppointmentDetailsModel Appointment { get; set; }
        public Guid CheckupId { get; set; }
#nullable disable

        public void OnGet()
        {
            var appointment = _appointmentRepository.GetAppointmentByIdAsync(Id).Result;
            Appointment = _appointmentService.GetAppointmentDetailsById(appointment.Id);
            Checkups = _checkupService.GetAvailableHospitalCheckupModelsForPatient(appointment.Patient.Id);
        }
    }
}
