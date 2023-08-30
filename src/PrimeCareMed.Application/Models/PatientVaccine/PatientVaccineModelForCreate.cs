using System.ComponentModel.DataAnnotations;

namespace PrimeCareMed.Application.Models.PatientVaccine
{
    public class PatientVaccineModelForCreate
    {
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string VaccineId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Dosage (mg)")]
        public string Dosage { get; set; }
    }
}
