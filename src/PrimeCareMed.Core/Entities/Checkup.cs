using PrimeCareMed.Core.Common;

namespace PrimeCareMed.Core.Entities
{
    public class Checkup : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string Preparation { get; set; }
#nullable enable
        public ICollection<HospitalCheckup>? HospitalCheckups { get; set; } = new List<HospitalCheckup>();
#nullable disable
    }
}
