using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.GeneralMedicineOffice;
using PrimeCareMed.Application.Models.Shift;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Application.Services;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.Shift
{
    [Authorize(Roles = "Nurse, Doctor, SysAdministrator")]
    public class CreateShiftModel : PageModel
    {

        private readonly IShiftService _shiftSessionService;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IOfficeService _officeService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateShiftModel(
            IMapper mapper,
            IUserService userService,
            IUserRepository userRepository,
            IShiftService shiftSessionService,
            IOfficeService officeService,
            UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _userService = userService;
            _userRepository = userRepository;
            _shiftSessionService = shiftSessionService;
            _officeService = officeService;
            _userManager = userManager;

        }
        [BindProperty]
        public ShiftModelForCreate NewShift { get; set; }
        [BindProperty]
        public IEnumerable<ListUsersModel> Doctors => _userService.GetAllUsers().Where(r => r.UserRole == "Doctor");
        [BindProperty]
        public IEnumerable<ListUsersModel> Nurses => _userService.GetAllUsers().Where(r => r.UserRole == "Nurse");
        [BindProperty]
        public IEnumerable<OfficeModel> Offices => _officeService.GetAllOffices();
        [BindProperty]
        public ApplicationUser CurrentUser => _userManager.GetUserAsync(HttpContext.User).Result;
     
        public async Task<IActionResult> OnPostAsync()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var currentUserRole = _userManager.GetRolesAsync(currentUser).Result.First();
            Console.WriteLine($"SESTRA {NewShift.NurseId}");
            
            if (!ModelState.IsValid) { return Page(); }
            
            else if (await _shiftSessionService.CheckIfShiftExists(NewShift.OfficeId, NewShift.NurseId, NewShift.DoctorId))
            {
                
                ViewData["Message"] = string.Format("Shift session already exists");
                return Page();
            }
            else
            {
                
                try
                {
                    Console.WriteLine($"CODEBEHIND {NewShift.DoctorId}");
                    Console.WriteLine($"CODEBEHIND {NewShift.NurseId}");
                    Console.WriteLine($"CODEBEHIND {NewShift.OfficeId}");
                    await _shiftSessionService.AddAsync(NewShift);
                    return Redirect("./ChooseShift");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR");
                    Console.WriteLine(ex.Message);
                }
            }
            return Page();
        }
        
    }
}
