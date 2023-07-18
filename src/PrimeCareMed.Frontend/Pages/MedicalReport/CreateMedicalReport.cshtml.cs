using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using PrimeCareMed.Application.Models.MedicalReport;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Application.Services;
using PrimeCareMed.DataAccess.Repositories;
using System.Data;

namespace PrimeCareMed.Frontend.Pages.MedicalReport
{
    [Authorize(Roles = "Doctor, SysAdministrator")]
    public class CreateMedicalReportModel : PageModel
    {
        private readonly IMedicineRepository _medicineRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IMedicalReportService _medicalReportService;
        public CreateMedicalReportModel(IMedicineRepository medicineRepository,
            IMapper mapper,
            IUserService userService,
            IUserRepository userRepository,
            IMedicalReportService medicalReportService)
        {
            _medicineRepository = medicineRepository;
            _mapper = mapper;
            _userService = userService;
            _userRepository = userRepository;
            _medicalReportService = medicalReportService;

        }
        [FromRoute]
        public Guid Id { get; set; }

        [BindProperty]
        public MedicalReportModelForCreate NewMedicalReport { get; set; }

    }
}
