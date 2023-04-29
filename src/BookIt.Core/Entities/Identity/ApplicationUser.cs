using Microsoft.AspNetCore.Identity;

namespace BookIt.Core.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
#nullable enable
        public Restaurant? Restaurant { get; set; }
#nullable disable
        public ICollection<Reservation> CustomerReservations { get; set; } = new List<Reservation>();

    }
}
