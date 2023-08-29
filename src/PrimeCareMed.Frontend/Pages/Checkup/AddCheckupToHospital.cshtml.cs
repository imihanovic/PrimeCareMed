using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Checkup;
using PrimeCareMed.Application.Models.Hospital;
using PrimeCareMed.Application.Models.HospitalCheckup;
using PrimeCareMed.Application.Services;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.Checkup
{
    [Authorize(Roles = "Administrator, SysAdministrator")]
    public class AddCheckupToHospitalModel : PageModel
    {
        private readonly IMedicineService _medicineService;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IHospitalService _hospitalService;
        private readonly ICheckupService _checkupService;
        public AddCheckupToHospitalModel(IMedicineService medicineService,
            IMapper mapper,
            IUserService userService,
            IUserRepository userRepository,
            IHospitalService hospitalService,
            ICheckupService checkupService)
        {
            _medicineService = medicineService;
            _mapper = mapper;
            _userService = userService;
            _userRepository = userRepository;
            _hospitalService = hospitalService;
            _checkupService = checkupService;

        }
        [FromRoute]
        public Guid Id { get; set; }

        [BindProperty]
        public HospitalCheckupModelForCreate NewHospitalCheckup { get; set; }
        public HospitalModel Hospital => _hospitalService.GetHospitalModelById(Id.ToString());

        [BindProperty]
        public IEnumerable<CheckupModel> Checkups => _checkupService.GetAllCheckupsNotInHospital(Id);

        public async Task<IActionResult> OnPostAsync(List<string> checkups)
        {
            NewHospitalCheckup.HospitalId = Id;
            foreach(var checkup in checkups)
            {
                NewHospitalCheckup.CheckupId = Guid.Parse(checkup);
                try
                {
                    await _checkupService.AddHospitalCheckup(NewHospitalCheckup);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Page();
                }
            }
            
            return RedirectToPage("/Checkup/ViewAllCheckupsForHospital", new { id = Id });
        }
    }
}
