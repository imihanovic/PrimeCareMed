using System.ComponentModel.DataAnnotations;

namespace PrimeCareMed.Application.Models.Vaccine
{
    public class VaccineModelForCreate
    {
        public string Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Side effects")]
        public string SideEffects { get; set; }
    }
}
