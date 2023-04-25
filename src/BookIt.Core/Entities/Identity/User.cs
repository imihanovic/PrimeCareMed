using Microsoft.AspNetCore.Identity;

namespace BookIt.Core.Entities.Identity
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<Reservation> ManagerReservations { get; set; }= new List<Reservation>();

        public ICollection<Reservation> CustomerReservations { get; set; } = new List<Reservation>();

    }
}
