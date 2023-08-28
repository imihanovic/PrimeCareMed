using PrimeCareMed.Core.Enums;

namespace PrimeCareMed.Application.Models.Patient
{
    public class PatientModel : BaseResponseModel
    {
        public string Mbo { get; set; }
        public string Oib { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Doctor { get; set; }
    }
}
