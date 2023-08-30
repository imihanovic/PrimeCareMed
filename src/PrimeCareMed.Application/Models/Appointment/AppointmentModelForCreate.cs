using System.ComponentModel.DataAnnotations;

namespace PrimeCareMed.Application.Models.Appointment
{
    public class AppointmentModelForCreate
    {
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Cause")]
        public string Cause { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Shift")]
        public string ShiftId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Patient")]
        public string PatientId { get; set; }
#nullable enable
        [DataType(DataType.Text)]
        [Display(Name = "Medical report")]
        public string? MedicalReport { get; set; }
#nullable disable
    }
}
