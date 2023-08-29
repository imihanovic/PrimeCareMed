using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Application.Models.CheckupAppointment;
using PrimeCareMed.Application.Models.MedicinePrescription;
using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Application.Models.PatientVaccine;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.Patients
{
    [Authorize(Roles = "Doctor, Nurse, SysAdministrator")]
    public class PatientDetailsModel : PageModel
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;
        private readonly ICheckupAppointmentService _checkupAppointmentService;
        private readonly IMedicinePrescriptionService _medicinePrescriptionService;
        private readonly IPatientVaccineService _patientVaccineService;
        private readonly IAppointmentService _appointmentService;

        public int TotalPages { get; set; }
        public PatientDetailsModel(
            IPatientRepository patientRepository,
            IMapper mapper,
            ICheckupAppointmentService checkupAppointmentService,
            IMedicinePrescriptionService medicinePrescriptionService,
            IPatientVaccineService patientVaccineService,
            IAppointmentService appointmentService)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
            _checkupAppointmentService = checkupAppointmentService;
            _medicinePrescriptionService = medicinePrescriptionService;
            _patientVaccineService = patientVaccineService;
            _appointmentService = appointmentService;
        }
        [FromRoute]
        public Guid Id { get; set; }

        [BindProperty]
        public PatientModel Patient { get; set; }

#nullable enable
        [BindProperty]
        public IEnumerable<CheckupAppointmentModel>? Checkups { get; set; } 

        [BindProperty]
        public IEnumerable<MedicinePrescriptionModel>? Medicines { get; set; }
        [BindProperty]
        public IEnumerable<PatientVaccineModel>? Vaccines { get; set; }
        [BindProperty]
        public IEnumerable<AppointmentModel>? Appointments { get; set; }
#nullable disable
        public void OnGet()
        {
            var patient = _patientRepository.GetPatientByIdAsync(Id).Result;
            Patient = _mapper.Map<PatientModel>(patient);
            Checkups = _checkupAppointmentService.GetAllPatientCheckupAppointments(Id).Take(3);
            Medicines = _medicinePrescriptionService.GetMedicinePrescriptionsForPatient(Id).Take(3);
            Vaccines = _patientVaccineService.GetPatientVaccineForPatient(Id).Take(3);
            Appointments = _appointmentService.GetAllAppointmentsForPatient(Id.ToString()).Take(3);
        }
    }
}
