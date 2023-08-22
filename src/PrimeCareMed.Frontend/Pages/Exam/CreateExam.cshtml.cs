using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Exam;
using PrimeCareMed.Application.Services;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.Exam
{
    public class CreateExamModel : PageModel
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IExamService _examService;
        public CreateExamModel(IOfficeRepository officeRepository,
            IMapper mapper,
            IUserService userService,
            IUserRepository userRepository,
            IExamService examService)
        {
            _officeRepository = officeRepository;
            _mapper = mapper;
            _userService = userService;
            _userRepository = userRepository;
            _examService = examService;

        }
        [BindProperty]
        public ExamModelForCreate NewExam { get; set; }

        public async Task<IActionResult> OnPostAsync(string Description, string Preparation)
        {
            NewExam.Description = Description;
            NewExam.Preparation = Preparation;
            try
            {
                await _examService.AddAsync(NewExam);
                return RedirectToPage("ViewAllExams");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Page();
        }
    }
}
