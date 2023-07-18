using System.ComponentModel.DataAnnotations;

namespace PrimeCareMed.Application.Models.MedicalReport
{
    public class MedicalReportModelForCreate
    {
        
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
