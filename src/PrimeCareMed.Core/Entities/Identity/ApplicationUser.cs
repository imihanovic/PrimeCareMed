using Microsoft.AspNetCore.Identity;
using PrimeCareMed.Core.Enums;

namespace PrimeCareMed.Core.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
            public string FirstName { get; set; }
            public string LastName { get; set; }
#nullable enable
            public ICollection<Shift>? DoctorsShifts { get; set; } = new List<Shift>();
            public ICollection<Shift>? NursesShifts { get; set; } = new List<Shift>();
#nullable disable
    }
}
