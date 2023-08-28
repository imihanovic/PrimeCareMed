using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Application.Services;
using System.Data;

namespace PrimeCareMed.Frontend.Pages.Patients
{
    [Authorize(Roles = "Nurse, Doctor, SysAdministrator")]
    public class CreatePatientModel : PageModel
    {
        
        private readonly IPatientService _patientService;
        private readonly IUserService _userService;
        
        public CreatePatientModel(
            IPatientService patientService,
            IUserService userService)
        {
            _patientService = patientService;
            _userService = userService;
        }
        [BindProperty]
        public PatientModelForCreate NewPatient { get; set; }

        public IEnumerable<ListUsersModel> Doctors => _userService.GetAllUsers().Where(r=>r.UserRole == "Doctor");

        public async Task<IActionResult> OnPostAsync()
        {
            if(NewPatient.Oib.Length != 11)
            {
                ViewData["Message"] = string.Format("OIB is invalid!");
                return Page();
            }
            else if(NewPatient.Mbo.Length != 9)
            {
                ViewData["Message"] = string.Format("MBO is invalid!");
                return Page();
            }
            try
            {
                NewPatient.DateOfBirth = NewPatient.DateOfBirth.ToUniversalTime();
                await _patientService.AddAsync(NewPatient);
                return RedirectToPage("ViewAllPatients");
            }
            catch (Exception ex)
            {
                ViewData["Message"] = string.Format("OIB or MBO is already taken!");
                Console.WriteLine(ex.Message);
            }
            return Page();
        }
    }
}
