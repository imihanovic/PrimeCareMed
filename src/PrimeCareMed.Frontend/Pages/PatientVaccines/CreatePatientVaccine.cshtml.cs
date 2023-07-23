using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Application.Models.MedicinePrescription;
using PrimeCareMed.Application.Models.PatientVaccine;
using PrimeCareMed.Application.Models.Vaccine;
using PrimeCareMed.Application.Services;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.PatientVaccines
{
    public class CreatePatientVaccineModel : PageModel
    {
        private readonly IVaccineService _vaccineService;
        private readonly IPatientVaccineService _patientVaccineService;
        public CreatePatientVaccineModel(IVaccineService vaccineService,
            IPatientVaccineService patientVaccineService)
        {
            _vaccineService = vaccineService;
            _patientVaccineService = patientVaccineService;

        }
        [FromRoute]
        public Guid Id { get; set; }

        [BindProperty]
        public PatientVaccineModelForCreate NewPatientVaccine { get; set; }

        [BindProperty]
        public IEnumerable<VaccineModel> Vaccines => _vaccineService.GetAllVaccines();
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _patientVaccineService.AddAsync(NewPatientVaccine, Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Page();
            }
            return RedirectToPage("../Appointment/ViewAppointmentDetails", new { id = Id });
        }
    }
}
