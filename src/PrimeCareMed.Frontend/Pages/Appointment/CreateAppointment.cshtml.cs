using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Application.Models.Shift;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;
using System.Data;

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
        private readonly IShiftRepository _shiftRepository;
        public readonly UserManager<ApplicationUser> _userManager;
        public CreateAppointmentModel(IMedicineRepository medicineRepository,
            IMapper mapper,
            IUserService userService,
            IUserRepository userRepository,
            IAppointmentService appointmentService,
            IPatientService patientService,
            IShiftService shiftService,
            UserManager<ApplicationUser> userManager,
            IShiftRepository shiftRepository
            )
        {
            _medicineRepository = medicineRepository;
            _mapper = mapper;
            _userService = userService;
            _userRepository = userRepository;
            _appointmentService = appointmentService;
            _patientService = patientService;
            _shiftService = shiftService;
            _userManager = userManager;
            _shiftRepository = shiftRepository;

        }
        [BindProperty]
        public AppointmentModelForCreate NewAppointment { get; set; }

        [BindProperty(SupportsGet = true)]
        public IEnumerable<PatientModel> Patients { get; set; }

        [BindProperty(SupportsGet = true)]
        public ShiftModel CurrentShift { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CurrentShiftId { get; set; }

        [BindProperty]
        public IEnumerable<ShiftModel> Shifts => _shiftService.GetAllShifts().Where(r=>r.ShiftEndTime is null);
        public IActionResult OnGet()
        {
            var cookie = Request.Cookies["sessionCookie"];
            if(cookie != null)
            {
                var shift = _shiftService.GetShiftById(cookie);
                CurrentShift = shift;
                CurrentShiftId = cookie;
                Patients = _patientService.GetAllAvailablePatients(cookie);
            }
            else
            {
                Patients = _patientService.GetAllPatients("");
            }
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            var doctorShift = _shiftRepository.CheckIfOpenShiftExistsForDoctor(currentUser.Id);
            var nurseShift = _shiftRepository.CheckIfOpenShiftExistsForNurse(currentUser.Id);
            if (currentUserRole == "Doctor" && doctorShift is null)
            {
                return Redirect("/Shift/CreateShift");
            }
            else if (currentUserRole == "Nurse" && nurseShift is null)
            {
                return Redirect("/Shift/CreateShift");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string cause)
        {
            NewAppointment.Cause = cause;
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
