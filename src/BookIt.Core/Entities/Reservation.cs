using BookIt.Core.Common;
using BookIt.Core.Entities.Identity;
using BookIt.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookIt.Core.Entities
{
    public class Reservation : BaseEntity
    {
        public User Manager { get; set; }
#nullable enable
        public User? Customer { get; set; }
#nullable disable
        public int NumberOfPersons { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public ReservationStatus Status { get; set; }
        
        public List<Table> Tables { get; set; } = new();

    }
}
