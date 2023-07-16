using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Application.Models.GeneralMedicineOffice;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Application.Models.Shift;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace PrimeCareMed.Frontend.Pages.Appointment
{
    [Authorize(Roles = "Doctor, Nurse, SysAdministrator")]
    public class CreateAppointmentModel : PageModel
    {
        
        private readonly IMedicineRepository _medicineRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;
        private readonly IShiftService _shiftService;
        private readonly ISessionRepository _sessionRepository;
        public CreateAppointmentModel(IMedicineRepository medicineRepository,
            IMapper mapper,
            IUserService userService,
            IUserRepository userRepository,
            IAppointmentService appointmentService,
            IPatientService patientService,
            IShiftService shiftService,
            ISessionRepository sessionRepository
            )
        {
            _medicineRepository = medicineRepository;
            _mapper = mapper;
            _userService = userService;
            _userRepository = userRepository;
            _appointmentService = appointmentService;
            _patientService = patientService;
            _shiftService = shiftService;
            _sessionRepository = sessionRepository;

        }
        [BindProperty]
        public AppointmentModelForCreate NewAppointment { get; set; }

        [BindProperty(SupportsGet = true)]
        public IEnumerable<PatientModel> Patients { get; set; }

        [BindProperty(SupportsGet = true)]
        public ShiftModel CurrentShift { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CurrentSessionId { get; set; }

        [BindProperty]
        public IEnumerable<ShiftModel> Shifts => _shiftService.GetAllShifts();
        public IActionResult OnGet()
        {
            var cookie = Request.Cookies["sessionCookie"];
            if(cookie != null)
            {
                var session = _sessionRepository.GetSessionByIdAsync(Guid.Parse(cookie)).Result;
                CurrentShift = _shiftService.GetShiftById(session.Shift.Id.ToString());
                CurrentSessionId = cookie;
                Patients = _patientService.GetAllAvailablePatients(session.Id.ToString());
            }
            else
            {
                Patients = _patientService.GetAllPatients();
            }
            Console.WriteLine($"KUKI KOD PACIJENATA{cookie}");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _appointmentService.AddAsync(NewAppointment);
                if (User.IsInRole("SysAdministrator"))
                {
                    return RedirectToPage("ViewAllAppointments");
                }
                else
                {
                    return RedirectToPage("WaitingRoom");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Page();
        }

        
    }
}
