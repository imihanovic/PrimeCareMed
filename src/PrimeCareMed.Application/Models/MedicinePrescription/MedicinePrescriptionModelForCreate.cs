using System.ComponentModel.DataAnnotations;

namespace PrimeCareMed.Application.Models.MedicinePrescription
{
    public class MedicinePrescriptionModelForCreate
    {
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Medicine")]
        public string MedicineId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }

    }
}
