using System.ComponentModel.DataAnnotations;

namespace BookIt.Application.Models.Table
{
    public class TableModelForUpdate
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
    }
}
