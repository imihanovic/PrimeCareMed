using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Application.Services;
using PrimeCareMed.DataAccess.Repositories;
using System.Data;

namespace PrimeCareMed.Frontend.Pages.Medicine
{
    [Authorize(Roles = "Administrator, SysAdministrator")]
    public class CreateMedicineModel : PageModel
    {   
            private readonly IMedicineRepository _medicineRepository;
            private readonly IUserRepository _userRepository;
            private readonly IUserService _userService;
            private readonly IMapper _mapper;
            private readonly IMedicineService _medicineService;
            public CreateMedicineModel(IMedicineRepository medicineRepository,
                IMapper mapper,
                IUserService userService,
                IUserRepository userRepository,
                IMedicineService medicineService)
            {
                _medicineRepository = medicineRepository;
                _mapper = mapper;
                _userService = userService;
                _userRepository = userRepository;
                _medicineService = medicineService;
            
            }
            [BindProperty]
            public MedicineModelForCreate NewMedicine { get; set; }

            public async Task<IActionResult> OnPostAsync(string description)
            {
                NewMedicine.Description = description;
                try
                {
                    await _medicineService.AddAsync(NewMedicine);
                    return RedirectToPage("ViewAllMedicines");
                }
                catch (Exception ex)
                {
                    ViewData["Message"] = "Invalid form";
                    return Page();
                }
            }
        
    }
}
