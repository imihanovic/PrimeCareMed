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
#nullable enable
        public ICollection<Session>? Sessions { get; set; } = new List<Session>();
#nullable disable
    }
}
