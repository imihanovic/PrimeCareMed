using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Application.Services.Impl;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;
using PrimeCareMed.DataAccess.Repositories.Impl;
using System.Data;

namespace PrimeCareMed.Frontend.Pages.Appointment
{
    [Authorize(Roles = "Doctor, SysAdministrator")]
    public class EditAppointmentDetailsModel : PageModel
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IAppointmentService _appointmentService;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IShiftRepository _shiftRepository;

        public EditAppointmentDetailsModel(IAppointmentRepository appointmentRepository,
            IAppointmentService appointmentService,
            IUserRepository userRepository,
            IUserService userService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IShiftRepository shiftRepository)
        {
            _appointmentRepository = appointmentRepository;
            _appointmentService = appointmentService;
            _userService = userService;
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
            _shiftRepository = shiftRepository;
        }
        [FromRoute]
        public Guid Id { get; set; }

        [BindProperty]
        public AppointmentModelForCreate EditAppointment { get; set; }

        [BindProperty]
        public AppointmentDetailsModel Appointment { get; set; }

        public IActionResult OnGet()
        {
            Appointment = _appointmentService.GetAppointmentDetailsById(Id);
            var appointment = _appointmentRepository.GetAppointmentByIdAsync(Id).Result;
            EditAppointment = _mapper.Map<AppointmentModelForCreate>(appointment);
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            var doctorShift = _shiftRepository.CheckIfOpenShiftExistsForDoctor(currentUser.Id);
            if (currentUserRole == "Doctor" && doctorShift is null)
            {
                return Redirect("/Shift/CreateShift");
            }
            return Page();
        }
        public IActionResult OnPost(string medicalReport)
        {
            //if (!ModelState.IsValid) { Console.WriteLine("MODELSTATE"); return Page(); }
            EditAppointment.Id = Id.ToString();
            EditAppointment.MedicalReport = medicalReport;
            try
            {
                _appointmentService.EditAppointmentAsync(EditAppointment);
                //return RedirectToPage("ViewAppointmentDetails", new {id=Id});
                return RedirectToPage("/Appointment/ViewAppointmentDetails", new {id=Id});
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION");
                Console.WriteLine(ex.Message);
                return Page();
            }
        }

        public IActionResult OnPostDelete()
        {
            _appointmentService.DeleteAppointmentAsync(Id);

            return RedirectToPage("ViewAllPatients");
        }
    }
}
