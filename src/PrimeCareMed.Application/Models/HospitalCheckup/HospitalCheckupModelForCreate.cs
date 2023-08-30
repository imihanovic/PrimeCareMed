using System.ComponentModel.DataAnnotations;

namespace PrimeCareMed.Application.Models.HospitalCheckup
{
    public class HospitalCheckupModelForCreate
    {
        [Required]
        [Display(Name = "HospitalId")]
        public Guid HospitalId { get; set; }

        [Required]
        [Display(Name = "CheckupId")]
        public Guid CheckupId { get; set; }
    }
}
