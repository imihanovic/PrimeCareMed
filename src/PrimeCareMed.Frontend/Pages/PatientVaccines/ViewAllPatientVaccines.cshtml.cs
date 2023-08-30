using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.PatientVaccine;
using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace PrimeCareMed.Frontend.Pages.PatientVaccines
{
    [Authorize(Roles = "Doctor, Nurse, SysAdministrator")]
    public class ViewAllPatientVaccinesModel : PageModel
    {
        private readonly IPatientVaccineService _patientVaccineService;
        public readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IPatientRepository _patientRepository;

        public List<string> PatientVaccineModelProperties { get; set; }
        public PaginatedList<PatientVaccineModel> Vaccines { get; set; }
        public PatientModel Patient { get; set; }
        public int TotalPages { get; set; }
        [FromRoute]
        public Guid Id { get; set; }
        public ViewAllPatientVaccinesModel(
            UserManager<ApplicationUser> userManager,
            IPatientVaccineService patientVaccineService,
            IMapper mapper,
            IPatientRepository patientRepository
            )
        {
            _userManager = userManager;
            _patientVaccineService = patientVaccineService;
            _mapper = mapper;
            _patientRepository = patientRepository;

        }
        public void OnGet(string sort, string keyword, int? pageIndex)

        {
            PatientVaccineModelProperties = _patientVaccineService.GetPatientVaccineModelFields();
            var patient = _patientRepository.GetPatientByIdAsync(Id).Result;
            Patient = _mapper.Map<PatientModel>(patient);
            var vaccines = _patientVaccineService.GetPatientVaccineForPatient(Id);

            if (keyword != null)
            {
                pageIndex = 1;
            }

            int pageSize = 8;

            ViewData["CurrentSort"] = sort;
            vaccines = _patientVaccineService.VaccineSorting(vaccines, sort);

            ViewData["Keyword"] = keyword;
            vaccines = _patientVaccineService.VaccineSearch(vaccines, keyword);

            Vaccines = PaginatedList<PatientVaccineModel>.Create(vaccines, pageIndex ?? 1, pageSize);
            TotalPages = (int)Math.Ceiling(decimal.Divide(vaccines.Count(), pageSize));
        }
    }
}
