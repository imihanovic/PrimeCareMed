using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Checkup;
using PrimeCareMed.Application.Services;

namespace PrimeCareMed.Frontend.Pages.Checkup
{
    [Authorize(Roles = "Administrator, SysAdministrator")]
    public class CreateCheckupModel : PageModel
    {
        private readonly ICheckupService _checkupService;
        public CreateCheckupModel(
            ICheckupService checkupService)
        {
            _checkupService = checkupService;
        }
        [BindProperty]
        public CheckupModelForCreate NewCheckup { get; set; }

        public async Task<IActionResult> OnPostAsync(string Description, string Preparation)
        {
            NewCheckup.Description = Description;
            NewCheckup.Preparation = Preparation;
            try
            {
                await _checkupService.AddAsync(NewCheckup);
                return RedirectToPage("ViewAllCheckups");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Page();
            }
            
        }
    }
}
