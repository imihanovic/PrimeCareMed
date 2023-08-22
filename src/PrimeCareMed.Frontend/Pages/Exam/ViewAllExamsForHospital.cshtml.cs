using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Exam;
using PrimeCareMed.Application.Models.Hospital;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.Exam
{
    public class ViewAllExamsForHospitalModel : PageModel
    {
        public readonly IExamService _examService;
        public readonly IOfficeRepository _officeRepository;
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly IShiftService _shiftService;
        public readonly IHospitalService _hospitalService;

        public List<string> ExamModelProperties;
        public PaginatedList<ExamModel> Exams { get; set; }
        public HospitalModel Hospital { get; set; }
        public int TotalPages { get; set; }

        [FromRoute]
        public Guid Id { get; set; }

        public ViewAllExamsForHospitalModel(IExamService examService,
            UserManager<ApplicationUser> userManager,
            IOfficeRepository officeRepository,
            IShiftService shiftService,
            IHospitalService hospitalService
            )
        {
            _examService = examService;
            _userManager = userManager;
            _officeRepository = officeRepository;
            _shiftService = shiftService;
            _hospitalService = hospitalService;

        }
        public void OnGet(string sort, string currentFilter, string keyword, int? pageIndex)

        {
            Hospital = _hospitalService.GetHospitalModelById(Id.ToString());
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            ExamModelProperties = _examService.GetExamModelFields();

            if (keyword != null)
            {
                pageIndex = 1;
            }
            else
            {
                keyword = currentFilter;
            }
            ViewData["CurrentFilter"] = keyword;
            int pageSize = 7;

            var exams = _examService.GetAllHospitalExams(Id);

            ViewData["CurrentSort"] = sort;
            // SORTIRANJE PACIJENATA
            exams = _examService.ExamSorting(exams, sort);

            ViewData["Keyword"] = keyword;
            exams = _examService.ExamSearch(exams, keyword);

            Exams = PaginatedList<ExamModel>.Create(exams, pageIndex ?? 1, pageSize);

            TotalPages = (int)Math.Ceiling(decimal.Divide(exams.Count(), pageSize));
        }
    }
}
