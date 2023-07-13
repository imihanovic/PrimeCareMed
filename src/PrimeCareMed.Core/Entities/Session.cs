using PrimeCareMed.Core.Common;
using PrimeCareMed.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.Core.Entities
{
    public class Session : BaseEntity
    {
            public Shift Shift { get; set; }
            public DateTime ShiftStartTime { get; set; }
            public DateTime ShiftEndTime { get; set; }
#nullable enable
            public ICollection<Appointment>? Appointments { get; set; } = new List<Appointment>();
#nullable disable
    }
}
