using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.MedicinePrescription;
using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.Medicine
{
    [Authorize(Roles = "Doctor, Nurse, SysAdministrator")]
    public class ViewAllPatientMedicinesModel : PageModel
    {
        private readonly IMedicinePrescriptionService _medicinePrescriptionService;
        public readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IPatientRepository _patientRepository;

        public List<string> MedicinePrescriptionModelProperties { get; set; }
        public PaginatedList<MedicinePrescriptionModel> Medicines { get; set; }
        public PatientModel Patient { get; set; }
        public int TotalPages { get; set; }
        [FromRoute]
        public Guid Id { get; set; }
        public ViewAllPatientMedicinesModel(
            UserManager<ApplicationUser> userManager,
            IMedicinePrescriptionService medicinePrescriptionService,
            IMapper mapper,
            IPatientRepository patientRepository
            )
        {
            _userManager = userManager;
            _medicinePrescriptionService = medicinePrescriptionService;
            _mapper = mapper;
            _patientRepository = patientRepository;

        }
        public void OnGet(string sort, string keyword, int? pageIndex)

        {
            MedicinePrescriptionModelProperties = _medicinePrescriptionService.GetMedicinePrescriptionModelFields();
            var patient = _patientRepository.GetPatientByIdAsync(Id).Result;
            Patient = _mapper.Map<PatientModel>(patient);
            var medicines = _medicinePrescriptionService.GetMedicinePrescriptionsForPatient(Id);

            if (keyword != null)
            {
                pageIndex = 1;
            }

            int pageSize = 8;

            ViewData["CurrentSort"] = sort;
            medicines = _medicinePrescriptionService.MedicinePrescriptionSorting(medicines, sort);

            ViewData["Keyword"] = keyword;
            medicines = _medicinePrescriptionService.MedicinePrescriptionSearch(medicines, keyword);

            Medicines = PaginatedList<MedicinePrescriptionModel>.Create(medicines, pageIndex ?? 1, pageSize);
            TotalPages = (int)Math.Ceiling(decimal.Divide(medicines.Count(), pageSize));
        }
    }
}
