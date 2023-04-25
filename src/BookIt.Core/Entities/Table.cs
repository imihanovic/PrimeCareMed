using BookIt.Core.Common;
using BookIt.Core.Enums;

namespace BookIt.Core.Entities
{
    public class Table : BaseEntity
    {
        public int NumberOfSeats { get; set; }
        
        public TableArea Area { get; set; }

        public TableSmoking Smoking { get; set; }

        public List<Reservation> Reservations { get; set; } = new();
    }
}
