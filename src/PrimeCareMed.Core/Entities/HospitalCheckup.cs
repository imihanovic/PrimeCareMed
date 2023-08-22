using PrimeCareMed.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PrimeCareMed.Core.Entities
{
    public class HospitalCheckup
    {
        public Guid HospitalId { get; set; }

        public Guid CheckupId { get; set; }

        [ForeignKey("HospitalId")]
        public Hospital Hospital { get; set; }

        [ForeignKey("CheckupId")]
        public Checkup Checkup { get; set; }
#nullable enable
        public ICollection<CheckupAppointment>? HospitalCheckups { get; set; } = new List<CheckupAppointment>();
#nullable disable

    }
}
