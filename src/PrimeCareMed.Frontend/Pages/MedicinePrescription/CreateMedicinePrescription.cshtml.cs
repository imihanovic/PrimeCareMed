using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Application.Models.MedicinePrescription;
using PrimeCareMed.Application.Services;

namespace PrimeCareMed.Frontend.Pages.MedicinePrescription
{
    [Authorize(Roles = "Doctor, Nurse, SysAdministrator")]
    public class CreateMedicinePrescriptionModel : PageModel
    { 
        private readonly IMedicineService _medicineService;
        private readonly IMedicinePrescriptionService _medicinePrescriptionService;
        public CreateMedicinePrescriptionModel(IMedicineService medicineService,
            IMedicinePrescriptionService medicinePrescriptionService)
        {
            _medicineService = medicineService;
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

