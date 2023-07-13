using System.ComponentModel.DataAnnotations;

namespace PrimeCareMed.Application.Models.Medicine
{
    public class MedicineModelForCreate
    {
        public string Id { get; set; }

        
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}

