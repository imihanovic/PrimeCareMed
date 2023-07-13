using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.Medicine
{
    public class ViewAllMedicinesModel : PageModel
    {
        public readonly IMedicineService _medicineService;
        public readonly IMedicineRepository _medicineRepository;
        public readonly UserManager<ApplicationUser> _userManager;
        
        public List<string> MedicineModelProperties;
        public List<MedicineModel> Medicines { get; set; }
        public int TotalPages { get; set; }

        public ViewAllMedicinesModel(IMedicineService medicineService,
            UserManager<ApplicationUser> userManager,
            IMedicineRepository medicineRepository
            )
        {
            _medicineService = medicineService;
            _userManager = userManager;
            _medicineRepository = medicineRepository;

        }
        public void OnGet()

        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            MedicineModelProperties = _medicineService.GetMedicineModelFields();

          

            var medicines = _medicineService.GetAllMedicines();
            Medicines = medicines.ToList();
            //ViewData["Keyword"] = keyword;
            //dishes = _dishService.DishSearch(dishes, keyword);

            //ViewData["CategoryFilter"] = categoryFilter;
            //dishes = _dishService.DishFilter(dishes, categoryFilter);

            //Dishes = PaginatedList<DishModel>.Create(dishes, pageIndex ?? 1, pageSize);

            //TotalPages = (int)Math.Ceiling(decimal.Divide(dishes.Count(), pageSize));

        }
    }
}
