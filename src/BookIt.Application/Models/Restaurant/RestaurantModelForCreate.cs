using System.ComponentModel.DataAnnotations;

namespace BookIt.Application.Models.Restaurant
{
    public class RestaurantModelForCreate
    {
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Restaurant owner*")]
        public string RestaurantOwner { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Restaurant name*")]
        public string RestaurantName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "City*")]
        public string City { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Address*")]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Manager*")]
        public string ManagerId { get; set; }
    }
}
