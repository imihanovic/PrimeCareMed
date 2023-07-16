using PrimeCareMed.Core.Common;
using PrimeCareMed.Core.Entities.Identity;

namespace PrimeCareMed.Core.Entities
{
    public class GeneralMedicineOffice : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
#nullable enable
        public ICollection<Shift>? Shifts { get; set; } = new List<Shift>();
#nullable disable
    }
}
