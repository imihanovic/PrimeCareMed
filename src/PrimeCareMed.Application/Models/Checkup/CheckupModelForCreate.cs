using System.ComponentModel.DataAnnotations;

namespace PrimeCareMed.Application.Models.Checkup
{
    public class CheckupModelForCreate
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

        [Required]
        [Range(1, Int32.MaxValue)]
        [Display(Name = "Duration")]
        public int Duration { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Preparation { get; set; }
    }
}
