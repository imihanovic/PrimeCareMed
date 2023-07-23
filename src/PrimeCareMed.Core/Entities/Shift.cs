using PrimeCareMed.Core.Common;
using PrimeCareMed.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.Core.Entities
{
    public class Shift : BaseEntity
    {
        public ApplicationUser Doctor { get; set; }
        public ApplicationUser Nurse { get; set; }
        public GeneralMedicineOffice Office { get; set; }
        public DateTime ShiftStartTime { get; set; }
#nullable enable
        public DateTime? ShiftEndTime { get; set; }
        public ICollection<Appointment>? Appointments { get; set; } = new List<Appointment>();
#nullable disable
        public Shift() { }
    }
}
