using PrimeCareMed.Core.Common;

namespace PrimeCareMed.Core.Entities
{
    public class Hospital : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
#nullable enable
        public ICollection<HospitalCheckup>? HospitalCheckups { get; set; } = new List<HospitalCheckup>();
#nullable disable
    }
}
