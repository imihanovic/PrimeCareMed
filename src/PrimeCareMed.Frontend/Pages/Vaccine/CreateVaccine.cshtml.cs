using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Vaccine;
using PrimeCareMed.Application.Services;
using PrimeCareMed.DataAccess.Repositories;
using System.Data;

namespace PrimeCareMed.Frontend.Pages.Vaccine
{
    [Authorize(Roles = "Administrator, SysAdministrator")]
        
    public class CreateVaccineModel : PageModel
    {
        private readonly IVaccineRepository _vaccineRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IVaccineService _vaccineService;
        public CreateVaccineModel(IVaccineRepository vaccineRepository,
            IMapper mapper,
            IUserService userService,
            IUserRepository userRepository,
            IVaccineService vaccineService)
        {
            _vaccineRepository = vaccineRepository;
            _mapper = mapper;
            _userService = userService;
            _userRepository = userRepository;
            _vaccineService = vaccineService;

        }
        [BindProperty]
        public VaccineModelForCreate NewVaccine { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { return Page(); }

            try
            {
                await _vaccineService.AddAsync(NewVaccine);
                return RedirectToPage("ViewAllVaccines");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Page();
        }

    }
}
