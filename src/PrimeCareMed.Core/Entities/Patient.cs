using PrimeCareMed.Core.Common;
using PrimeCareMed.Core.Entities.Identity;
using PrimeCareMed.Core.Enums;

namespace PrimeCareMed.Core.Entities
{
    public class Patient : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Oib { get; set; }
        public string Mbo { get; set; }
        public Gender Gender { get; set; }
#nullable enable
        public ICollection<Appointment>? Appointments { get; set; } = new List<Appointment>();
        public ApplicationUser? Doctor { get; set; }
#nullable disable
    }
}
