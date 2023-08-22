using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Exam;
using PrimeCareMed.Application.Models.Hospital;
using PrimeCareMed.Application.Models.HospitalExam;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Application.Models.MedicinePrescription;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Application.Services.Impl;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.Exam
{
    public class AddExamToHospitalModel : PageModel
    {
        private readonly IMedicineService _medicineService;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IHospitalService _hospitalService;
        private readonly IExamService _examService;
        public AddExamToHospitalModel(IMedicineService medicineService,
            IMapper mapper,
            IUserService userService,
            IUserRepository userRepository,
            IHospitalService hospitalService,
            IExamService examService)
        {
            _medicineService = medicineService;
            _mapper = mapper;
            _userService = userService;
            _userRepository = userRepository;
            _hospitalService = hospitalService;
            _examService = examService;

        }
        [FromRoute]
        public Guid Id { get; set; }

        [BindProperty]
        public HospitalExamModelForCreate NewHospitalExam { get; set; }
        public HospitalModel Hospital => _hospitalService.GetHospitalModelById(Id.ToString());

        [BindProperty]
        public IEnumerable<ExamModel> Exams => _examService.GetAllExamsNotInHospital(Id);

        public async Task<IActionResult> OnPostAsync()
        {
            NewHospitalExam.HospitalId = Id;
            try
            {
                await _examService.AddHospitalExam(NewHospitalExam);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Page();
            }
            return RedirectToPage("/Hospital/ViewAllExamsForHospital", new { id = Id });
        }
    }
}
