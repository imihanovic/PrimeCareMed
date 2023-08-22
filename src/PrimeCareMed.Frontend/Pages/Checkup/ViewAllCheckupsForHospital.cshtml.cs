using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Checkup;
using PrimeCareMed.Application.Models.Hospital;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.Checkup
{
    public class ViewAllCheckupsForHospitalModel : PageModel
    {
        public readonly ICheckupService _checkupService;
        public readonly IOfficeRepository _officeRepository;
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly IShiftService _shiftService;
        public readonly IHospitalService _hospitalService;

        public List<string> CheckupModelProperties;
        public PaginatedList<CheckupModel> Checkups { get; set; }
        public HospitalModel Hospital { get; set; }
        public int TotalPages { get; set; }

        [FromRoute]
        public Guid Id { get; set; }

        public ViewAllCheckupsForHospitalModel(ICheckupService checkupService,
            UserManager<ApplicationUser> userManager,
            IOfficeRepository officeRepository,
            IShiftService shiftService,
            IHospitalService hospitalService
            )
        {
            _checkupService = checkupService;
            _userManager = userManager;
            _officeRepository = officeRepository;
            _shiftService = shiftService;
            _hospitalService = hospitalService;

        }
        public void OnGet(string sort, string currentFilter, string keyword, int? pageIndex)

        {
            Hospital = _hospitalService.GetHospitalModelById(Id.ToString());
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            CheckupModelProperties = _checkupService.GetCheckupModelFields();

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

            var checkups = _checkupService.GetAllHospitalCheckups(Id);

            ViewData["CurrentSort"] = sort;
            // SORTIRANJE PACIJENATA
            checkups = _checkupService.CheckupSorting(checkups, sort);

            ViewData["Keyword"] = keyword;
            checkups = _checkupService.CheckupSearch(checkups, keyword);

            Checkups = PaginatedList<CheckupModel>.Create(checkups, pageIndex ?? 1, pageSize);

            TotalPages = (int)Math.Ceiling(decimal.Divide(checkups.Count(), pageSize));
        }
    }
}
