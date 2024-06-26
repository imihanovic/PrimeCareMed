﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Application.Models.Vaccine;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.Vaccine
{
    [Authorize(Roles = "Administrator, SysAdministrator")]
    public class ViewAllVaccinesModel : PageModel
    {
        public readonly IVaccineService _vaccineService;
        public readonly IVaccineRepository _vaccineRepository;
        public readonly UserManager<ApplicationUser> _userManager;

        public List<string> VaccineModelProperties;
        public PaginatedList<VaccineModel> Vaccines { get; set; }
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
        public void OnGet(string sort, string keyword, int? pageIndex)

        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            VaccineModelProperties = _vaccineService.GetVaccineModelFields();


            if (keyword != null)
            {
                pageIndex = 1;
            }
            
            int pageSize = 7;

            var vaccines = _vaccineService.GetAllVaccines();

            ViewData["CurrentSort"] = sort;
            vaccines = _vaccineService.VaccineSorting(vaccines, sort);

            ViewData["Keyword"] = keyword;
            vaccines = _vaccineService.VaccineSearch(vaccines, keyword);

            Vaccines = PaginatedList<VaccineModel>.Create(vaccines, pageIndex ?? 1, pageSize);

            TotalPages = (int)Math.Ceiling(decimal.Divide(vaccines.Count(), pageSize));
        }
    }
}
