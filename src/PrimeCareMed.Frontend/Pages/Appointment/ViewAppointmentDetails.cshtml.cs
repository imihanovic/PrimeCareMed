using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Application.Models.MedicinePrescription;
using PrimeCareMed.Application.Models.PatientVaccine;
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
        public readonly IPatientVaccineService _patientVaccineService;
        public readonly IMedicinePrescriptionService _medicinePrescriptionService;
        public readonly IMedicinePrescriptionRepository _medicinePrescriptionRepository;
        public readonly IPatientVaccineRepository _patientVaccineRepository;

        [BindProperty]
        public AppointmentDetailsModel Appointment { get; set; }
#nullable enable

        [BindProperty]
        public IEnumerable<PatientVaccineModel>? PatientVaccines { get; set; }

        [BindProperty]
        public IEnumerable<MedicinePrescriptionModel>? MedicinePrescriptions { get; set; }
#nullable disable

        [FromRoute]
        public Guid Id { get; set; }
        public int TotalPages { get; set; }

        public bool PatientsDoctor { get; set; }

        public ViewAppointmentDetailsModel(IAppointmentService appointmentService,
            UserManager<ApplicationUser> userManager,
            IAppointmentRepository appointmentRepository,
            IPatientVaccineService patientVaccineService,
            IMedicinePrescriptionService medicinePrescriptionService, 
            IPatientVaccineRepository patientVaccineRepository,
            IMedicinePrescriptionRepository medicinePrescriptionRepository)
        {
            _appointmentService = appointmentService;
            _userManager = userManager;
            _appointmentRepository = appointmentRepository;
            _patientVaccineService = patientVaccineService;
            _medicinePrescriptionService = medicinePrescriptionService;
            _patientVaccineRepository = patientVaccineRepository;
            _medicinePrescriptionRepository = medicinePrescriptionRepository;
        }
        public IActionResult OnGet()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            
            Appointment = _appointmentService.GetAppointmentDetailsById(Id);
            var appointment = _appointmentRepository.GetAppointmentByIdAsync(Id).Result;
            
            
            var cookie = Request.Cookies["sessionCookie"];
            if (currentUserRole == "Doctor" && cookie is null)
            {
                return Redirect("/Shift/CreateShift");
            }
            else if (currentUserRole == "Nurse" && cookie is null)
            {
                return Redirect("/Shift/CreateShift");
            }
            if (_patientVaccineRepository.CheckPatientsVaccinesForAppointmentAsync(Id))
            {
                PatientVaccines = _patientVaccineService.GetPatientVaccineForAppointment(Id);
            }
            if (_medicinePrescriptionRepository.CheckMedicinePrescriptionForAppointmentAsync(Id))
            {
                MedicinePrescriptions = _medicinePrescriptionService.GetMedicinePrescriptionsForAppointment(Id);
            }
            if(appointment.Patient.Doctor is not null)
            {
                PatientsDoctor = currentUser.Id == appointment.Patient.Doctor.Id ? true : false;
            }
            else
            {
                PatientsDoctor = false;
            }
            
            return Page();
        }
        public IActionResult OnPost()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            try
            {
                var appointmentDB = _appointmentRepository.GetAppointmentByIdAsync(Id).Result;
                appointmentDB.Status = Core.Enums.AppointmentStatus.Done;
                _appointmentRepository.FinishAppointmentAsync(appointmentDB);
                if(currentUserRole == "SysAdministrator")
                {
                    return RedirectToPage("/Appointment/ViewAllAppointments");
                }
                return RedirectToPage("/Appointment/WaitingRoom");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Page();
            }
        }

    }
}
