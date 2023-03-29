using BookIt.Core.Common;
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
        public Guid UserId { get; set; }

        public string NumberOfPersons { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public ReservationStatusEnum Status { get; set; }
    }
}
