using PrimeCareMed.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace PrimeCareMed.Application.Models.Patient
{
    public class PatientModelForCreate
    {
        public string Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Oib")]
        public string Oib { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Mbo")]
        public string Mbo { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }
    }
}
