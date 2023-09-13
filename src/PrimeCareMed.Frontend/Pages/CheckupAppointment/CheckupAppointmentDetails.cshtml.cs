using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Application.Models.CheckupAppointment;
using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.CheckupAppointment
{
    public class CheckupAppointmentDetailsModel : PageModel
    {
        private readonly ICheckupAppointmentService _checkupAppointmentService;
        private readonly ICheckupAppointmentRepository _checkupAppointmentRepository;
        public readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IPatientRepository _patientRepository;

        public CheckupAppointmentModel Checkup { get; set; }
        public PatientModel Patient { get; set; }
        public int TotalPages { get; set; }

        [FromRoute]
        public Guid Id { get; set; }
        public CheckupAppointmentDetailsModel(
            UserManager<ApplicationUser> userManager,
            ICheckupAppointmentService checkupAppointmentService,
            IMapper mapper,
            IPatientRepository patientRepository,
            ICheckupAppointmentRepository checkupAppointmentRepository
            )
        {
            _userManager = userManager;
            _checkupAppointmentService = checkupAppointmentService;
            _mapper = mapper;
            _patientRepository = patientRepository;
            _checkupAppointmentRepository = checkupAppointmentRepository;

        }
        public void OnGet()
        {
            var checkup = _checkupAppointmentRepository.GetCheckupAppointmentByIdAsync(Id.ToString()).Result;
            Checkup = _checkupAppointmentService.GetCheckupAppointmentDetailsById(Id.ToString());
            var patient = _patientRepository.GetPatientByIdAsync(checkup.Appointment.Patient.Id).Result;
            Patient = _mapper.Map<PatientModel>(patient);
        }
        public IActionResult OnPost()
        {
            var checkup = _checkupAppointmentRepository.GetCheckupAppointmentByIdAsync(Id.ToString()).Result;
            checkup.CheckupStatus = Core.Enums.CheckupStatus.Cancelled;
            var appointmentUpdate = _checkupAppointmentRepository.UpdateAsync(checkup).Result;
            return RedirectToPage("/Checkup/ViewAllPatientCheckups", new { id = checkup.Appointment.Patient.Id });
        }
    }
}
