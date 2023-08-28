using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Hospital;
using PrimeCareMed.Application.Services;

namespace PrimeCareMed.Frontend.Pages.Hospital
{
    [Authorize(Roles = "Administrator, SysAdministrator")]
    public class CreateHospitalModel : PageModel
    {
        private readonly IHospitalService _hospitalService;
        public CreateHospitalModel(
            IHospitalService hospitalService)
        {
            _hospitalService = hospitalService;
        }
        [BindProperty]
        public HospitalModelForCreate NewHospital { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { return Page(); }

            try
            {
                await _hospitalService.AddAsync(NewHospital);
                return RedirectToPage("ViewAllHospitals");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Page();
        }
    }
}
