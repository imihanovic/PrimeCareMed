using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.CheckupAppointment;
using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Application.Services.Impl;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.Checkup
{
    [Authorize(Roles = "Doctor, Nurse, SysAdministrator")]
    public class ViewAllPatientCheckupsModel : PageModel
    {
        private readonly ICheckupAppointmentService _checkupAppointmentService;
        public readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IPatientRepository _patientRepository;

        public List<string> CheckupAppointmentModelProperties { get; set; }
        public PaginatedList<CheckupAppointmentModel> Checkups { get; set; }
        public PatientModel Patient { get; set; }
        public int TotalPages { get; set; }
        [FromRoute]
        public Guid Id { get; set; }
        public ViewAllPatientCheckupsModel(
            UserManager<ApplicationUser> userManager,
            ICheckupAppointmentService checkupAppointmentService,
            IMapper mapper,
            IPatientRepository patientRepository
            )
        {
            _userManager = userManager;
            _checkupAppointmentService = checkupAppointmentService;
            _mapper = mapper;
            _patientRepository = patientRepository;

        }
        public void OnGet(string sort, string keyword, int? pageIndex)

        {
            CheckupAppointmentModelProperties = _checkupAppointmentService.GetCheckupAppointmentModelFields();
            var patient = _patientRepository.GetPatientByIdAsync(Id).Result;
            Patient = _mapper.Map<PatientModel>(patient);
            var checkups = _checkupAppointmentService.GetAllPatientCheckupAppointments(Id);

            if (keyword != null)
            {
                pageIndex = 1;
            }

            int pageSize = 8;

            ViewData["CurrentSort"] = sort;
            checkups = _checkupAppointmentService.CheckupAppointmentSorting(checkups, sort);

            ViewData["Keyword"] = keyword;
            checkups = _checkupAppointmentService.CheckupAppointmentSearch(checkups, keyword);

            Checkups = PaginatedList<CheckupAppointmentModel>.Create(checkups, pageIndex ?? 1, pageSize);
            TotalPages = (int)Math.Ceiling(decimal.Divide(checkups.Count(), pageSize));
        }
        public IActionResult OnPostAsync()
        {
            _checkupAppointmentService.DeleteCheckupAppointmentAsync(Id);

            return RedirectToPage("ViewAllPatientCheckups");
        }
    }
}
