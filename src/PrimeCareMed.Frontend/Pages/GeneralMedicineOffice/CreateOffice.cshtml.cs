using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.GeneralMedicineOffice;
using PrimeCareMed.Application.Services;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.GeneralMedicineOffice
{
    [Authorize(Roles = "Administrator, SysAdministrator")]
    public class CreateOfficeModel : PageModel
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IOfficeService _officeService;
        public CreateOfficeModel(IOfficeRepository officeRepository,
            IMapper mapper,
            IUserService userService,
            IUserRepository userRepository,
            IOfficeService officeService)
        {
            _officeRepository = officeRepository;
            _mapper = mapper;
            _userService = userService;
            _userRepository = userRepository;
            _officeService = officeService;

        }
        [BindProperty]
        public OfficeModelForCreate NewOffice { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { return Page(); }
            try
            {
                await _officeService.AddAsync(NewOffice);
                return RedirectToPage("ViewAllOffices");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Page();
        }

    }
}
