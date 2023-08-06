using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using PrimeCareMed.Application.Models.GeneralMedicineOffice;
using PrimeCareMed.Application.Models.Shift;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Application.Services.Impl;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;
using PrimeCareMed.DataAccess.Repositories.Impl;

namespace PrimeCareMed.Frontend.Pages.Shift
{
    [Authorize(Roles = "Nurse, Doctor, SysAdministrator")]
    public class CreateShiftModel : PageModel
    {

        private readonly IShiftService _shiftService;
        private readonly IShiftRepository _shiftRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IOfficeService _officeService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateShiftModel(
            IMapper mapper,
            IUserService userService,
            IUserRepository userRepository,
            IShiftService shiftService,
            IOfficeService officeService,
            UserManager<ApplicationUser> userManager,
            IShiftRepository shiftRepository)
        {
            _mapper = mapper;
            _userService = userService;
            _userRepository = userRepository;
            _shiftService = shiftService;
            _officeService = officeService;
            _userManager = userManager;
            _shiftRepository = shiftRepository;
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
            //int pageSize = 9;
            var doctorShift = _shiftRepository.CheckIfOpenShiftExistsForDoctor(currentUser.Id);
            var nurseShift = _shiftRepository.CheckIfOpenShiftExistsForNurse(currentUser.Id);

            // PROVJERIT MED SESTRU I DOKTORA U TRENUTNOM COOKIJU!!
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
                   
                    Console.WriteLine($"SHIFT");
                    //HttpContext.Response.Cookies.Append(
                    //"sessionCookie", shift.Id.ToString(),
                    //new CookieOptions() { SameSite = SameSiteMode.Lax });
                    //HttpContext.Response.Cookies.Append(
                    //"shiftCookieOffice", shift.Office.Name + " " + shift.Office.City,
                    //new CookieOptions() { SameSite = SameSiteMode.Lax });
                    //if (currentUserRole == "Doctor")
                    //{
                    //    HttpContext.Response.Cookies.Append(
                    //     "shiftCookieDetails", shift.Nurse.FirstName + " " + shift.Nurse.LastName,
                    //     new CookieOptions() { SameSite = SameSiteMode.Lax });
                    //}
                    //else
                    //{
                    //    HttpContext.Response.Cookies.Append(
                    //    "shiftCookieDetails", shift.Doctor.FirstName + " " + shift.Doctor.LastName,
                    //    new CookieOptions() { SameSite = SameSiteMode.Lax });
                    //}

                    return Redirect("../Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR");
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
            if(currentUserRole == "Doctor")
            {
                shift = _shiftRepository.CheckIfOpenShiftExistsForDoctor(currentUser.Id);
            }
            else
            {
                shift = _shiftRepository.CheckIfOpenShiftExistsForDoctor(currentUser.Id);
            }
            Response.Cookies.Delete("shiftCookie");
            Response.Cookies.Delete("sessionCookie");
            Response.Cookies.Delete("shiftCookieOffice");
            Response.Cookies.Delete("shiftCookieDetails");
            shift = _shiftRepository.UpdateAsync(shift).Result;
            return Redirect("/Shift/CreateShift");
        }

    }
}
