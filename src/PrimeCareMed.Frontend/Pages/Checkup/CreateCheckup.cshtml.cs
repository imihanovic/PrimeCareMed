using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Checkup;
using PrimeCareMed.Application.Services;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.Checkup
{
    public class CreateCheckupModel : PageModel
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ICheckupService _checkupService;
        public CreateCheckupModel(IOfficeRepository officeRepository,
            IMapper mapper,
            IUserService userService,
            IUserRepository userRepository,
            ICheckupService checkupService)
        {
            _officeRepository = officeRepository;
            _mapper = mapper;
            _userService = userService;
            _userRepository = userRepository;
            _checkupService = checkupService;

        }
        [BindProperty]
        public CheckupModelForCreate NewCheckup { get; set; }

        public async Task<IActionResult> OnPostAsync(string Description, string Preparation)
        {
            NewCheckup.Description = Description;
            NewCheckup.Preparation = Preparation;
            try
            {
                await _checkupService.AddAsync(NewCheckup);
                return RedirectToPage("ViewAllCheckups");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Page();
        }
    }
}
