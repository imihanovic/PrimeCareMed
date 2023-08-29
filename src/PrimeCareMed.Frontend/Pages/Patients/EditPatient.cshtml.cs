using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Application.Services;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.Patients
{
    [Authorize(Roles = "Doctor, Nurse, SysAdministrator")]
    public class EditPatientModel : PageModel
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;

        public EditPatientModel(IPatientRepository patientRepository,
            IPatientService patientService,
            IMapper mapper)
        {
            _patientRepository = patientRepository;
            _patientService = patientService;
            _mapper = mapper;
        }

        [FromRoute]
        public Guid Id { get; set; }

        [BindProperty]
        public PatientModelForCreate EditPatient { get; set; }

        public void OnGet()
        {
            var patient = _patientRepository.GetPatientByIdAsync(Id).Result;
            EditPatient = _mapper.Map<PatientModelForCreate>(patient);
        }
        public IActionResult OnPostEdit()
        {
            if (!ModelState.IsValid) { return Page(); }
            else if (EditPatient.Mbo.Length != 9 || EditPatient.Oib.Length != 11)
            {
                ViewData["Message"] = string.Format("OIB or MBO is invalid!");
                return Page();
            }
            EditPatient.Id = Id.ToString();
            try
            {
                _patientService.EditPatientAsync(EditPatient);
                return RedirectToPage("ViewAllPatients");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Page();
            }
        }

        public IActionResult OnPostDelete()
        {
            _patientService.DeletePatientAsync(Id);

            return RedirectToPage("ViewAllPatients");
        }
    }
}
