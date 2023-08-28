using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Vaccine;
using PrimeCareMed.Application.Services;

namespace PrimeCareMed.Frontend.Pages.Vaccine
{
    [Authorize(Roles = "Administrator, SysAdministrator")]  
    public class CreateVaccineModel : PageModel
    {
        private readonly IVaccineService _vaccineService;
        public CreateVaccineModel(
            IVaccineService vaccineService)
        {
            _vaccineService = vaccineService;
        }
        [BindProperty]
        public VaccineModelForCreate NewVaccine { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { return Page(); }

            try
            {
                await _vaccineService.AddAsync(NewVaccine);
                return RedirectToPage("ViewAllVaccines");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Page();
        }

    }
}
