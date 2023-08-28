using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages
{
    public class IndexModel : PageModel
    {
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly IShiftRepository _shiftRepository;

        public IndexModel(UserManager<ApplicationUser> userManager, IShiftRepository shiftRepository)
        {
            _userManager = userManager;
            _shiftRepository = shiftRepository;
        }
        [BindProperty]
        public string? OfficeString { get; set; }
        [BindProperty]
        public string? DoctorString { get; set; }
        [BindProperty]
        public string? NurseString { get; set; }
        public IActionResult OnGet()
        {
            try
            {
                var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
                var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
                var doctorShift = _shiftRepository.CheckIfOpenShiftExistsForDoctor(currentUser.Id);
                var nurseShift = _shiftRepository.CheckIfOpenShiftExistsForNurse(currentUser.Id);
                if (doctorShift is not null)
                {
                    OfficeString = doctorShift.Office.Name + ", " + doctorShift.Office.City;
                    NurseString = doctorShift.Nurse.FirstName + " " + doctorShift.Nurse.LastName;
                    HttpContext.Response.Cookies.Append(
                    "sessionCookie", doctorShift.Id.ToString(),
                    new CookieOptions() { SameSite = SameSiteMode.Lax });
                    HttpContext.Response.Cookies.Append(
                    "shiftCookieOffice", doctorShift.Office.Name + ", " + doctorShift.Office.City,
                    new CookieOptions() { SameSite = SameSiteMode.Lax });
                    HttpContext.Response.Cookies.Append(
                    "shiftCookieDetails", doctorShift.Nurse.FirstName + " " + doctorShift.Nurse.LastName,
                    new CookieOptions() { SameSite = SameSiteMode.Lax });
                }
                else if (nurseShift is not null)
                {
                    OfficeString = nurseShift.Office.Name + ", " + nurseShift.Office.City;
                    DoctorString = nurseShift.Doctor.FirstName + " " + nurseShift.Doctor.LastName;
                    HttpContext.Response.Cookies.Append(
                    "sessionCookie", nurseShift.Id.ToString(),
                    new CookieOptions() { SameSite = SameSiteMode.Lax });
                    HttpContext.Response.Cookies.Append(
                    "shiftCookieOffice", nurseShift.Office.Name + ", " + nurseShift.Office.City,
                    new CookieOptions() { SameSite = SameSiteMode.Lax });
                    HttpContext.Response.Cookies.Append(
                    "shiftCookieDetails", nurseShift.Doctor.FirstName + " " + nurseShift.Doctor.LastName,
                    new CookieOptions() { SameSite = SameSiteMode.Lax });
                }
                if (currentUserRole == "Doctor" && doctorShift is null)
                {
                    return Redirect("/Shift/CreateShift");
                }
                else if (currentUserRole == "Nurse" && nurseShift is null)
                {
                    return Redirect("/Shift/CreateShift");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ONGET INDEX {ex}");

            }
            return Page();
            
        }
    }
}
