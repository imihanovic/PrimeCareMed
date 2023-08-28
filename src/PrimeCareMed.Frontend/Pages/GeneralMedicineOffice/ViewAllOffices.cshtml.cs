using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.GeneralMedicineOffice;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.GeneralMedicineOffice
{
    [Authorize(Roles = "Administrator, SysAdministrator")]
    public class ViewAllOfficesModel : PageModel
    {
        public readonly IOfficeService _officeService;
        public readonly IOfficeRepository _officeRepository;
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly IShiftService _shiftService;

        public List<string> OfficeModelProperties;
        public PaginatedList<OfficeModel> Offices { get; set; }
        public int TotalPages { get; set; }

        public ViewAllOfficesModel(IOfficeService officeService,
            UserManager<ApplicationUser> userManager,
            IOfficeRepository officeRepository,
            IShiftService shiftService
            )
        {
            _officeService = officeService;
            _userManager = userManager;
            _officeRepository = officeRepository;
            _shiftService = shiftService;

        }
        public void OnGet(string sort, string currentFilter, string keyword, int? pageIndex)

        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            OfficeModelProperties = _officeService.GetOfficeModelFields();

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

            var offices = _officeService.GetAllOffices();

            ViewData["CurrentSort"] = sort;
            // SORTIRANJE PACIJENATA
            offices = _officeService.OfficeSorting(offices, sort);

            ViewData["Keyword"] = keyword;
            offices = _officeService.OfficeSearch(offices, keyword);

            Offices = PaginatedList<OfficeModel>.Create(offices, pageIndex ?? 1, pageSize);

            TotalPages = (int)Math.Ceiling(decimal.Divide(offices.Count(), pageSize));
        }
    }
}
