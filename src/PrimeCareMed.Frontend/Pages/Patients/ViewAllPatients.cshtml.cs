using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities.Identity;

namespace PrimeCareMed.Frontend.Pages.Patients
{
    [Authorize(Roles = "Doctor, Nurse, SysAdministrator")]
    public class ViewAllPatientsModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPatientService _patientService;

#nullable enable
        public PaginatedList<PatientModel> Patients { get; set; }
#nullable disable

        public List<string> PatientModelProperties;

        public int TotalPages { get; set; }
        public ViewAllPatientsModel(IUserService userService, UserManager<ApplicationUser> userManager, IPatientService patientService)
        {
            _userService = userService;
            _userManager = userManager;
            _patientService = patientService;
        }
        public void OnGet(string sort, string currentFilter, string keyword, string genderFilter, int? pageIndex)
        {

            PatientModelProperties = _patientService.GetPatientModelFields();

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

            var patients = _patientService.GetAllPatients();

            ViewData["CurrentSort"] = sort;
            patients = _patientService.PatientSorting(patients, sort);

            ViewData["Keyword"] = keyword;
            patients = _patientService.PatientSearch(patients, keyword);

            ViewData["GenderFilter"] = genderFilter;
            patients = _patientService.PatientFilter(patients, genderFilter);

            Patients = PaginatedList<PatientModel>.Create(patients, pageIndex ?? 1, pageSize);

            TotalPages = (int)Math.Ceiling(decimal.Divide(patients.Count(), pageSize));
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();

        }
        public IActionResult OnPostDelete(Guid id)
        {
            _patientService.DeletePatientAsync(id);
            return RedirectToPage("ViewAllPatients");
        }
    }
}
