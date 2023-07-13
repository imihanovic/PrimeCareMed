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
        public List<PatientModel> Patients { get; set; }
#nullable disable

        public List<string> PatientModelProperties;

        public int TotalPages { get; set; }
        public ViewAllPatientsModel(IUserService userService, UserManager<ApplicationUser> userManager, IPatientService patientService)
        {
            _userService = userService;
            _userManager = userManager;
            _patientService = patientService;
        }
        public IActionResult OnGet()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();

            PatientModelProperties = _patientService.GetPatientModelFields();

            //int pageSize = 9;

            //var restaurant = _restaurantRepository.GetRestaurantByIdAsync(Id).Result;

            //RestaurantModel = _mapper.Map<RestaurantModel>(restaurant);

            //if (currentUser.Restaurant != restaurant && currentUserRole == "Manager")
            //{
            //    return RedirectToPage("../Restaurant/ViewAllRestaurants");
            //}

            var patients = _patientService.GetAllPatients();

            //ViewData["CurrentSort"] = sort;
            //tables = _tableService.TableSorting(tables, sort);

            //ViewData["AreaFilter"] = areaFilter;
            //tables = _tableService.TableFilterByArea(tables, areaFilter);

            //ViewData["SmokingFilter"] = smokingFilter;
            //tables = _tableService.TableFilterBySmoking(tables, smokingFilter);

            Patients = patients.ToList();

            //Tables = PaginatedList<TableModel>.Create(tables, pageIndex ?? 1, pageSize);

            //TotalPages = (int)Math.Ceiling(decimal.Divide(tables.Count(), pageSize));

            return Page();
        }
        public IActionResult OnPostDelete(Guid id)
        {
            _patientService.DeletePatientAsync(id);
            return RedirectToPage("ViewAllPatients");
        }
    }
}
