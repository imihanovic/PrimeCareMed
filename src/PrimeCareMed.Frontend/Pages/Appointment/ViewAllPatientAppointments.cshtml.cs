using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.Appointment
{
    [Authorize(Roles = "Doctor, Nurse, SysAdministrator")]
    public class ViewAllPatientAppointmentsModel : PageModel
    {
        public readonly IAppointmentService _appointmentService;
        public readonly IAppointmentRepository _appointmentRepository;
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public List<string> AppointmentModelProperties;
        public PaginatedList<AppointmentModel> Appointments { get; set; }
        public int TotalPages { get; set; }
        public PatientModel Patient { get; set; }

        [FromRoute]
        public Guid Id { get; set; }
        public ViewAllPatientAppointmentsModel(IAppointmentService appointmentService,
            UserManager<ApplicationUser> userManager,
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IMapper mapper
            )
        {
            _appointmentService = appointmentService;
            _userManager = userManager;
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _mapper = mapper;
        }
        public IActionResult OnGet(string sort, string currentFilter, string keyword, int? pageIndex)

        {
            Console.WriteLine($"AJDI {Id}");
            var patient = _patientRepository.GetPatientByIdAsync(Id).Result;
            Patient = _mapper.Map<PatientModel>(patient);
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();

            AppointmentModelProperties = _appointmentService.GetAppointmentModelFields();
            var cookie = Request.Cookies["sessionCookie"];

            if (currentUserRole == "Doctor" && cookie is null)
            {
                return Redirect("/Shift/CreateShift");
            }
            else if (currentUserRole == "Nurse" && cookie is null)
            {
                return Redirect("/Shift/CreateShift");
            }
            var appointments = _appointmentService.GetAllAppointmentsForPatient(Id.ToString()).ToList();
            if (keyword != null)
            {
                pageIndex = 1;
            }
            else
            {
                keyword = currentFilter;
            }

            ViewData["CurrentFilter"] = keyword;
            int pageSize = 7;

            ViewData["CurrentSort"] = sort;

            appointments = _appointmentService.AppointmentSorting(appointments, sort).ToList();

            ViewData["Keyword"] = keyword;
            appointments = _appointmentService.AppointmentSearch(appointments, keyword).ToList();

            TotalPages = (int)Math.Ceiling(decimal.Divide(appointments.Count(), pageSize));
            Appointments = PaginatedList<AppointmentModel>.Create(appointments, pageIndex ?? 1, pageSize);

            return Page();
        }
    }
}
