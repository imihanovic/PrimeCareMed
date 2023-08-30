using System.ComponentModel.DataAnnotations;

namespace PrimeCareMed.Application.Models.Shift
{
    public class ShiftModelForCreate
    {
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Doctor")]
        public string DoctorId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nurse")]
        public string NurseId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "General Medicine Office")]
        public string OfficeId { get; set; }
    }
}
