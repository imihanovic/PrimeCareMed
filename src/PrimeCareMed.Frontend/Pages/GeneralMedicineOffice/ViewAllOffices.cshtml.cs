using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.GeneralMedicineOffice;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.GeneralMedicineOffice
{
    public class ViewAllOfficesModel : PageModel
    {
        public readonly IOfficeService _officeService;
        public readonly IOfficeRepository _officeRepository;
        public readonly UserManager<ApplicationUser> _userManager;

        public List<string> OfficeModelProperties;
        public List<OfficeModel> Offices { get; set; }
        public int TotalPages { get; set; }

        public ViewAllOfficesModel(IOfficeService officeService,
            UserManager<ApplicationUser> userManager,
            IOfficeRepository officeRepository
            )
        {
            _officeService = officeService;
            _userManager = userManager;
            _officeRepository = officeRepository;

        }
        public void OnGet()

        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            OfficeModelProperties = _officeService.GetOfficeModelFields();



            var offices = _officeService.GetAllOffices();
            Offices = offices.ToList();
            //ViewData["Keyword"] = keyword;
            //dishes = _dishService.DishSearch(dishes, keyword);

            //ViewData["CategoryFilter"] = categoryFilter;
            //dishes = _dishService.DishFilter(dishes, categoryFilter);

            //Dishes = PaginatedList<DishModel>.Create(dishes, pageIndex ?? 1, pageSize);

            //TotalPages = (int)Math.Ceiling(decimal.Divide(dishes.Count(), pageSize));

        }
    }
}
