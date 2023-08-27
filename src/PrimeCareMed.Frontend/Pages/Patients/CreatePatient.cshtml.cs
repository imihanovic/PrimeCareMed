using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Application.Services;
using PrimeCareMed.DataAccess.Repositories;
using System.Data;

namespace PrimeCareMed.Frontend.Pages.Patients
{
    [Authorize(Roles = "Nurse, Doctor, SysAdministrator")]
    public class CreatePatientModel : PageModel
    {
        
        private readonly IPatientService _patientService;
        
        public CreatePatientModel(
            IPatientService patientService)
        {
            _patientService = patientService;
        }
        [BindProperty]
        public PatientModelForCreate NewPatient { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { return Page(); }
            else if(NewPatient.Mbo.Length != 9 || NewPatient.Oib.Length != 11)
            {
                ViewData["Message"] = string.Format("OIB or MBO is invalid!");
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
