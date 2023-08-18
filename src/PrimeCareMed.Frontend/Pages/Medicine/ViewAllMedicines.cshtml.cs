using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Application.Models.User;
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
        public PaginatedList<MedicineModel> Medicines { get; set; }
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
        public void OnGet(string sort, string keyword, int? pageIndex)

        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            MedicineModelProperties = _medicineService.GetMedicineModelFields();

            if (keyword != null)
            {
                pageIndex = 1;
            }

            int pageSize = 8;

            var medicines = _medicineService.GetAllMedicines();

            ViewData["CurrentSort"] = sort;
            medicines = _medicineService.MedicineSorting(medicines, sort);

            ViewData["Keyword"] = keyword;
            medicines = _medicineService.MedicineSearch(medicines, keyword);

            Medicines = PaginatedList<MedicineModel>.Create(medicines, pageIndex ?? 1, pageSize);
            TotalPages = (int)Math.Ceiling(decimal.Divide(medicines.Count(), pageSize));
        }
    }
}
