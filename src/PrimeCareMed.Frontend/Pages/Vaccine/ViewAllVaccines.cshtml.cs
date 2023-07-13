using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Vaccine;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.Vaccine
{
    public class ViewAllVaccinesModel : PageModel
    {
        public readonly IVaccineService _vaccineService;
        public readonly IVaccineRepository _vaccineRepository;
        public readonly UserManager<ApplicationUser> _userManager;

        public List<string> VaccineModelProperties;
        public List<VaccineModel> Vaccines { get; set; }
        public int TotalPages { get; set; }

        public ViewAllVaccinesModel(IVaccineService vaccineService,
            UserManager<ApplicationUser> userManager,
            IVaccineRepository vaccineRepository
            )
        {
            _vaccineService = vaccineService;
            _userManager = userManager;
            _vaccineRepository = vaccineRepository;

        }
        public void OnGet()

        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            VaccineModelProperties = _vaccineService.GetVaccineModelFields();



            var vaccines = _vaccineService.GetAllVaccines();
            Vaccines = vaccines.ToList();
            //ViewData["Keyword"] = keyword;
            //dishes = _dishService.DishSearch(dishes, keyword);

            //ViewData["CategoryFilter"] = categoryFilter;
            //dishes = _dishService.DishFilter(dishes, categoryFilter);

            //Dishes = PaginatedList<DishModel>.Create(dishes, pageIndex ?? 1, pageSize);

            //TotalPages = (int)Math.Ceiling(decimal.Divide(dishes.Count(), pageSize));

        }
    }
}
