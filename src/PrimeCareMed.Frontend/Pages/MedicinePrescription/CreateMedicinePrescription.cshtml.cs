using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Application.Models.MedicinePrescription;
using PrimeCareMed.Application.Services;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Frontend.Pages.MedicinePrescription
{
    public class CreateMedicinePrescriptionModel : PageModel
    { 
        private readonly IMedicineService _medicineService;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IMedicinePrescriptionService _medicinePrescriptionService;
        public CreateMedicinePrescriptionModel(IMedicineService medicineService,
            IMapper mapper,
            IUserService userService,
            IUserRepository userRepository,
            IMedicinePrescriptionService medicinePrescriptionService)
        {
            _medicineService = medicineService;
            _mapper = mapper;
            _userService = userService;
            _userRepository = userRepository;
            _medicinePrescriptionService = medicinePrescriptionService;

        }
        [FromRoute]
        public Guid Id { get; set; }

        [BindProperty]
        public MedicinePrescriptionModelForCreate NewMedicinePrescription { get; set; }

        [BindProperty]
        public IEnumerable<MedicineModel> Medicines => _medicineService.GetAllMedicines();
        public async Task<IActionResult> OnPostAsync(string Description)
        {
            NewMedicinePrescription.Description = Description;
            try
            {
                await _medicinePrescriptionService.AddAsync(NewMedicinePrescription, Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Page();
            }
            return RedirectToPage("../Appointment/ViewAppointmentDetails", new { id = Id });
        }
        
    }

}

