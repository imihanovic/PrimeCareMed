using BookIt.Core.Common;
using BookIt.Core.Enums;

namespace BookIt.Core.Entities
{
    public class Table : BaseEntity
    {
        public string TableName { get; set; }

        public int NumberOfSeats { get; set; }
        
        public TableArea Area { get; set; }

        public TableSmoking Smoking { get; set; }

        public Restaurant Restaurant { get; set; }

        public List<Reservation> Reservations { get; set; } = new();
    }
}
