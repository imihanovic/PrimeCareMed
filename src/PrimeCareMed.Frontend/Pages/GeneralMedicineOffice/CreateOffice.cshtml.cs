using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.GeneralMedicineOffice;
using PrimeCareMed.Application.Services;

namespace PrimeCareMed.Frontend.Pages.GeneralMedicineOffice
{
    [Authorize(Roles = "Administrator, SysAdministrator")]
    public class CreateOfficeModel : PageModel
    {
        private readonly IOfficeService _officeService;
        public CreateOfficeModel(
            IOfficeService officeService)
        {
            _officeService = officeService;
        }
        [BindProperty]
        public OfficeModelForCreate NewOffice { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { return Page(); }
            try
            {
                await _officeService.AddAsync(NewOffice);
                return RedirectToPage("ViewAllOffices");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Page();
        }

    }
}
