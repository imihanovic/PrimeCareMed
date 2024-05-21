using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.GeneralMedicineOffice;
using PrimeCareMed.Application.Models.Shift;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Application.Services.Impl;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.Shift
{
    [Authorize(Roles = "Nurse, Doctor, SysAdministrator")]
    public class CreateShiftModel : PageModel
    {

        private readonly IShiftService _shiftService;
        private readonly IShiftRepository _shiftRepository;
        private readonly IOfficeService _officeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAppointmentService _appointmentService;
        public CreateShiftModel(
            IShiftService shiftService,
            IOfficeService officeService,
            UserManager<ApplicationUser> userManager,
            IShiftRepository shiftRepository,
            IAppointmentService appointmentService)
        {
            _shiftService = shiftService;
            _officeService = officeService;
            _userManager = userManager;
            _shiftRepository = shiftRepository;
            _appointmentService = appointmentService;
        }
        [BindProperty]
        public ShiftModelForCreate NewShift { get; set; }
        [BindProperty]
        public IEnumerable<ListUsersModel> Doctors => _shiftService.GetAllAvailableDoctors();
        [BindProperty]
        public IEnumerable<ListUsersModel> Nurses => _shiftService.GetAllAvailableNurses();
        [BindProperty]
        public IEnumerable<OfficeModel> Offices => _officeService.GetAllOffices();
        [BindProperty]
        public ApplicationUser CurrentUser => _userManager.GetUserAsync(HttpContext.User).Result;
        public IActionResult OnGet()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            var doctorShift = _shiftRepository.CheckIfOpenShiftExistsForDoctor(currentUser.Id);
            var nurseShift = _shiftRepository.CheckIfOpenShiftExistsForNurse(currentUser.Id);
            if (currentUserRole == "Doctor" && doctorShift is not null)
            {
                return Redirect("../Index");   
            }
            else if (currentUserRole == "Nurse" && nurseShift is not null )
            {
                return Redirect("../Index");
            }
            else if(currentUserRole == "SysAdministrator" || currentUserRole == "Administrator")
            {
                return Redirect("../Index");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            if (!ModelState.IsValid) {
                return Page(); }
            else
            {
                try
                {
                    var newShift = _shiftService.AddAsync(NewShift).Result;
                    HttpContext.Response.Cookies.Append(
                             "sessionCookie", newShift.Id.ToString(),
                             new CookieOptions() { SameSite = SameSiteMode.Lax });
                    HttpContext.Response.Cookies.Append(
                            "shiftCookie", newShift.OfficeName + " " + newShift.OfficeCity,
                            new CookieOptions() { SameSite = SameSiteMode.Lax });
                    HttpContext.Response.Cookies.Append(
                            "doctorId", NewShift.DoctorId,
                            new CookieOptions() { SameSite = SameSiteMode.Lax });
                    if (User.IsInRole("Doctor"))
                    {
                        HttpContext.Response.Cookies.Append(
                         "shiftCookieDetails", newShift.NurseFirstName + " " + newShift.NurseLastName,
                         new CookieOptions() { SameSite = SameSiteMode.Lax });
                    }
                    else if (User.IsInRole("Nurse"))
                    {
                        HttpContext.Response.Cookies.Append(
                         "shiftCookieDetails", newShift.DoctorFirstName + " " + newShift.DoctorLastName,
                         new CookieOptions() { SameSite = SameSiteMode.Lax });
                    }
                    return Redirect("../Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return Page();
        }

        public IActionResult OnPostEndShift()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            var shift = new PrimeCareMed.Core.Entities.Shift();
            var cookie = Request.Cookies["shiftCookie"];
            if (currentUserRole == "Doctor")
            {
                shift = _shiftRepository.CheckIfOpenShiftExistsForDoctor(currentUser.Id);
            }
            else
            {
                shift = _shiftRepository.CheckIfOpenShiftExistsForDoctor(currentUser.Id);
            }
            var openShifts = _appointmentService.GetAllAppointmentsForShift(shift.Id).Where(r => r.Status != "Done");

            if (openShifts.Any())
            {
                ViewData["ShowMessage"] = "There are undone appointments!";
                return Redirect("/Appointment/WaitingRoom");
            }
            Response.Cookies.Delete("shiftCookie");
            Response.Cookies.Delete("sessionCookie");
            Response.Cookies.Delete("shiftCookieOffice");
            Response.Cookies.Delete("shiftCookieDetails");
            Response.Cookies.Delete("doctorId");
            shift = _shiftRepository.UpdateAsync(shift).Result;
            return Redirect("/Shift/CreateShift");
        }

    }
}
