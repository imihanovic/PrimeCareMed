using PrimeCareMed.Core.Common;

namespace PrimeCareMed.Core.Entities
{
    public class Vaccine : BaseEntity
    {
        public string Name { get; set; }
        public string SideEffects { get; set; }
#nullable enable
        public ICollection<PatientsVaccine>? PatientsVaccines { get; set; } = new List<PatientsVaccine>();
#nullable disable
    }
}
