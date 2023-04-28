using BookIt.Core.Common;
using BookIt.Core.Entities.Identity;
using BookIt.Core.Enums;

namespace BookIt.Core.Entities
{
    public class Reservation : BaseEntity
    {
#nullable enable
        public User? Customer { get; set; }
#nullable disable
        public int NumberOfPersons { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public ReservationStatus Status { get; set; }
        
        public List<Table> Tables { get; set; } = new();

        public string ReservationDetails { get; set; }

    }
}
