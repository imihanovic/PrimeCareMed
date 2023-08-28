using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Application.Services;

namespace PrimeCareMed.Frontend.Pages.Medicine
{
    [Authorize(Roles = "Administrator, SysAdministrator")]
    public class CreateMedicineModel : PageModel
    {   
            private readonly IMedicineService _medicineService;
            public CreateMedicineModel(
                IMedicineService medicineService)
            {
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
