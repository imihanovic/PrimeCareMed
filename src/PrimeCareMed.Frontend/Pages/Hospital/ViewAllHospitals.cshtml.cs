using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Hospital;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.Hospital
{
    public class ViewAllHospitalsModel : PageModel
    {
        public readonly IHospitalService _hospitalService;
        public readonly IOfficeRepository _officeRepository;
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly IShiftService _shiftService;

        public List<string> HospitalModelProperties;
        public PaginatedList<HospitalModel> Hospitals { get; set; }
        public int TotalPages { get; set; }

        public ViewAllHospitalsModel(IHospitalService hospitalService,
            UserManager<ApplicationUser> userManager,
            IOfficeRepository officeRepository,
            IShiftService shiftService
            )
        {
            _hospitalService = hospitalService;
            _userManager = userManager;
            _officeRepository = officeRepository;
            _shiftService = shiftService;

        }
        public void OnGet(string sort, string currentFilter, string keyword, int? pageIndex)

        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            HospitalModelProperties = _hospitalService.GetHospitalModelFields();

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

            var hospitals = _hospitalService.GetAllHospitals();

            ViewData["CurrentSort"] = sort;
            // SORTIRANJE PACIJENATA
            hospitals = _hospitalService.HospitalSorting(hospitals, sort);

            ViewData["Keyword"] = keyword;
            hospitals = _hospitalService.HospitalSearch(hospitals, keyword);

            Hospitals = PaginatedList<HospitalModel>.Create(hospitals, pageIndex ?? 1, pageSize);

            TotalPages = (int)Math.Ceiling(decimal.Divide(hospitals.Count(), pageSize));
        }
    }
}
