using System.ComponentModel.DataAnnotations;

namespace BookIt.Application.Models.Table
{
    public class TableModelForCreate
    {
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Restaurant area*")]
        public string Area { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Smoking allowed*")]
        public string Smoking { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Restaurant*")]
        public string RestaurantId { get; set; }
    }
}
