using System.ComponentModel.DataAnnotations;

namespace PrimeCareMed.Application.Models.User
{
    public class UpdateUserModel
    {
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First name*")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Last name*")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Phone]
        [Display(Name = "Phone number*")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email*")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "User role*")]
        public string Role { get; set; }
    }
}
